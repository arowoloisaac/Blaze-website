using Arowolo_Delivery_Project.Data;
using Arowolo_Delivery_Project.Dtos.DishDto;
using Arowolo_Delivery_Project.Enums;
using Arowolo_Delivery_Project.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Arowolo_Delivery_Project.Services.DishService
{
    public class DishService : IDishService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public DishService(IMapper mapper, ApplicationDbContext context, UserManager<User> userManager)
        {
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
        }


        public async Task<List<GetDishDto>> AddDishes(AddDishDto newDish)
        {
            var dish = await _context.Dishes.FirstOrDefaultAsync( dish => dish.Name.ToLower() == newDish.Name.ToLower());

            if (dish is not null)
            {
                throw new ArgumentException($"Dish with {dish.Name} already exist");
            }

            _context.Dishes.Add(_mapper.Map<Dish>(newDish));
            await _context.SaveChangesAsync();

            var dbDishes = await _context.Dishes.ToListAsync();
            
            var mapperDishes = dbDishes.Select( dish => _mapper.Map<GetDishDto>(dish) ).ToList();

            return mapperDishes;
        }

        //for getting dish by ID
        public async Task<GetDishDto> GetDishById(Guid id)
        {
            var dish = await _context.Dishes.FirstOrDefaultAsync(d => d.Id == id);

            if (dish is null)
            {
                throw new Exception($"Dish with {id} doesn't exist");
            }
            return _mapper.Map<GetDishDto>(dish);
        }

        // for getting all the dishes
        public async Task<ServiceResponses> GetDishes([FromQuery] List<Category>? category, bool? vegetarian, Sorting? sort, int? page)
        {
            IQueryable<Dish> query = _context.Dishes;

            if (category != null && category.Any())
            {
                query = query.Where(d => category.Contains(d.Category));
            }
            

            if (vegetarian.HasValue)
            {
                query = query.Where(d => d.IsVegetarian == vegetarian);
            }

            if (sort != null)
            {
                switch (sort)
                {
                    case Sorting.NameAsc:
                        query = query.OrderBy(d => d.Name);
                        break;
                    case Sorting.NameDesc:
                        query = query.OrderByDescending(d => d.Name);
                        break;                     
                    case Sorting.PriceAsc:
                        query = query.OrderBy(filterBy => filterBy.Price);
                        break;
                    case Sorting.PriceDesc:
                        query = query.OrderByDescending(filterBy => filterBy.Price);
                        break;
                    case Sorting.RatingAsc:
                        query = query.OrderBy(filterBy => filterBy.Rating);
                        break;
                    case Sorting.RatingDesc:
                        query = query.OrderByDescending(filterBy => filterBy.Rating);
                        break;
                }
            }

            int pageResult = 5;
            int currentPage = page.HasValue && page > 0 ? page.Value : 1;

            int totalItems = await query.CountAsync();
            int pageCount = (int)Math.Ceiling((double)totalItems / pageResult);

            var dishes = await query.Skip((currentPage - 1) * pageResult)
                                    .Take(pageResult)
                                    .ToListAsync();

            totalItems = dishes.Count;

            var mappedDishes = _mapper.Map<IEnumerable<GetDishDto>>(dishes);

            var response = new ServiceResponses(mappedDishes.ToList(), currentPage, totalItems, pageCount);
            return response;
        }


        
        //for checking rating 
        public async Task<bool> GetDishRating(Guid dishId, string userId)
        {
            var currentUser = await _userManager.FindByIdAsync(userId);
            var dishRating = await _context.Rating.AnyAsync( rating => rating.DishId == dishId && rating.UserId == currentUser.Id);

            return dishRating;
        }


        //for adding rating to the dish
        public async Task<Rating> AddRating(Guid dishId, int ratingScore, string UserId)
        {
            var dishes = await _context.Dishes.FirstOrDefaultAsync(dish => dish.Id == dishId);
            var currentUser = await _userManager.FindByIdAsync(UserId);

            var newRating = new Rating
            {
                DishId = dishId,
                Value = ratingScore,
                UserId = currentUser.Id
            };

            _context.Rating.Add(newRating);
            await _context.SaveChangesAsync();

            return newRating;
        }

    }
}








/* 
 public async Task<ServiceResponses> GetDish(Category? category, bool? vegetarian, Sorting? sort, int? page)
        {
            IEnumerable<Dish> checkdish = await _context.Dishes.ToListAsync();
            IEnumerable<GetDishDto> filteredDishes = _mapper.Map<IEnumerable<GetDishDto>>(checkdish);

            int pageResult = 5;
            int currentPage = page.HasValue && page > 0 ? page.Value : 1;

            int totalItems;
            int pageCount;

            if (category == null || vegetarian == null || sort == null || page == null)
            {
                // filtereing for category
                if ((category != null && vegetarian == null && sort == null && page == null) || (category != null && vegetarian == null && sort == null && page != null))
                {
                    filteredDishes = filteredDishes.Where(filter => filter.Category == category);
                    //filteredDishes = filteredDishes.Where(filter => category.Contains(filter.Category));
                    var dish = filteredDishes.Skip((currentPage - 1) * pageResult).Take(pageResult).ToList();
                    
                    totalItems = dish.Count();
                    pageCount = ( totalItems/pageResult ) + 1;

                    dish.Select( dish => _mapper.Map<GetDishDto>(dish)).ToList();

                    var response = new ServiceResponses(dish, currentPage, totalItems, pageCount);
                    return response;
                }

                // filtereing for category, isVegetarian
                else if ((category != null && vegetarian != null && sort == null && page == null) || (category != null && vegetarian != null && sort == null && page != null))
                {
                    //filteredDishes = filteredDishes.Where((filter) => category.Contains(filter.Category));
                    filteredDishes = filteredDishes.Where(filter => filter.Category == category);
                    filteredDishes = filteredDishes.Where(filter => filter.IsVegetarian == vegetarian);

                    var dish = filteredDishes.Skip((currentPage -1) * pageResult).Take(pageResult).ToList();

                    totalItems = dish.Count();
                    pageCount = (totalItems / pageResult) + 1;

                    dish.Select( dish => _mapper.Map<GetDishDto>(dish)) .ToList();

                    var response = new ServiceResponses(dish, currentPage, totalItems, pageCount);
                    return response;
                }

                // filtereing for category, isVegetarian, sort
                //else if (category != null && vegetarian != null && sort != null && page == null)
                else if ((category != null && vegetarian != null && sort != null && page == null) || (category != null && vegetarian != null && sort != null && page != null))
                {
                    //var allCategories = category.SelectMany(c => c).ToList();
                    //filteredDishes = filteredDishes.Where(filter => category.Contains(filter.Category));
                    filteredDishes = filteredDishes.Where(filter => filter.Category == category);
                    filteredDishes = filteredDishes.Where(filter => filter.IsVegetarian == vegetarian);

                    switch(sort)
                    {
                        case Sorting.NameAsc:
                            filteredDishes = filteredDishes.OrderBy(filterBy => filterBy.Name);
                            break;

                        case Sorting.NameDesc:
                            filteredDishes = filteredDishes.OrderByDescending(filterBy => filterBy.Name);
                            break;

                        case Sorting.PriceAsc:
                            filteredDishes = filteredDishes.OrderBy(filterBy => filterBy.Price);
                            break;

                        case Sorting.PriceDesc:
                            filteredDishes = filteredDishes.OrderByDescending(filterBy => filterBy.Price);
                            break;

                        case Sorting.RatingAsc:
                            filteredDishes = filteredDishes.OrderBy(filterBy => filterBy.Rating);
                            break;

                        case Sorting.RatingDesc:
                            filteredDishes = filteredDishes.OrderByDescending(filterBy => filterBy.Rating);
                            break;
                    }

                    var dish = filteredDishes.Skip((currentPage - 1) * pageResult).Take(pageResult).ToList();

                    totalItems = dish.Count();
                    pageCount = ( totalItems/pageResult ) + 1;

                    dish.Select(dish => _mapper.Map<GetDishDto>(dish)).ToList();

                    var response = new ServiceResponses(dish, currentPage, totalItems, pageCount);
                    return response;
                }

                //filter for vegetarian 
                else if ((category == null && vegetarian != null && sort == null && page == null) || (category == null && vegetarian != null && sort == null && page != null))
                {
                    filteredDishes = filteredDishes.Where( filter => filter.IsVegetarian == vegetarian );

                    var dish = filteredDishes.Skip((currentPage - 1) * pageResult).Take(pageResult).ToList();

                    totalItems = dish.Count();
                    pageCount = ( totalItems/pageResult )+ 1;

                    dish.Select( dish => _mapper.Map<GetDishDto>(dish)).ToList();

                    var response = new ServiceResponses(dish, currentPage, totalItems, pageCount);
                    return response;
                }

                //filter for vegetarian and sort
                else if ((category == null && vegetarian != null && sort != null && page == null) || (category == null && vegetarian != null && sort != null && page != null))
                {
                    filteredDishes = filteredDishes.Where( filter => filter.IsVegetarian != vegetarian );

                    switch (sort)
                    {
                        case Sorting.NameAsc:
                            filteredDishes = filteredDishes.OrderBy(filterBy => filterBy.Name);
                            break;

                        case Sorting.NameDesc:
                            filteredDishes = filteredDishes.OrderByDescending(filterBy => filterBy.Name);
                            break;

                        case Sorting.PriceAsc:
                            filteredDishes = filteredDishes.OrderBy(filterBy => filterBy.Price);
                            break;

                        case Sorting.PriceDesc:
                            filteredDishes = filteredDishes.OrderByDescending(filterBy => filterBy.Price);
                            break;

                        case Sorting.RatingAsc:
                            filteredDishes = filteredDishes.OrderBy(filterBy => filterBy.Rating);
                            break;

                        case Sorting.RatingDesc:
                            filteredDishes = filteredDishes.OrderByDescending(filterBy => filterBy.Rating);
                            break;
                    }

                    var dish = filteredDishes.Skip((currentPage - 1) * pageResult).Take(pageResult).ToList();

                    totalItems = dish.Count();
                    pageCount = (totalItems / pageResult) + 1;

                    dish.Select(dish => _mapper.Map<GetDishDto>(dish)).ToList();

                    var response = new ServiceResponses(dish, currentPage, totalItems, pageCount);
                    return response;
                }

                //for sort
                else if ((category == null && vegetarian == null && sort != null && page == null) || (category == null && vegetarian == null && sort != null && page != null))
                {
                    switch (sort)
                    {
                        case Sorting.NameAsc:
                            filteredDishes = filteredDishes.OrderBy(filterBy => filterBy.Name);
                            break;

                        case Sorting.NameDesc:
                            filteredDishes = filteredDishes.OrderByDescending(filterBy => filterBy.Name);
                            break;

                        case Sorting.PriceAsc:
                            filteredDishes = filteredDishes.OrderBy(filterBy => filterBy.Price);
                            break;

                        case Sorting.PriceDesc:
                            filteredDishes = filteredDishes.OrderByDescending(filterBy => filterBy.Price);
                            break;

                        case Sorting.RatingAsc:
                            filteredDishes = filteredDishes.OrderBy(filterBy => filterBy.Rating);
                            break;

                        case Sorting.RatingDesc:
                            filteredDishes = filteredDishes.OrderByDescending(filterBy => filterBy.Rating);
                            break;
                    }

                    var dish = filteredDishes.Skip((currentPage - 1) * pageResult).Take(pageResult).ToList();

                    totalItems = dish.Count();
                    pageCount = (totalItems/pageResult) + 1;

                    dish.Select( dish => _mapper.Map<GetDishDto>(dish)).ToList();

                    var response = new ServiceResponses(dish, currentPage, totalItems, pageCount);
                    return response;
                }

                // for page
                else if (category == null && vegetarian == null && sort == null && page != null || page == null)
                {
                    var dish = filteredDishes.Skip((currentPage - 1) * pageResult).Take(pageResult).ToList();

                    totalItems = dish.Count();
                    pageCount = (totalItems / pageResult) + 1;

                    dish.Select(c => _mapper.Map<GetDishDto>(c)).ToList();

                    var response = new ServiceResponses(dish, currentPage, totalItems, pageCount);
                    return response;
                }
            }


            else
            {
                if(category != null && vegetarian != null && sort != null && page != null)
                {
                    //var allCategories = category.SelectMany(c => c).ToList();
                    filteredDishes = filteredDishes.Where(filter => filter.Category == category);
                    filteredDishes = filteredDishes.Where(filter => filter.IsVegetarian == vegetarian);

                    switch (sort)
                    {
                        case Sorting.NameAsc:
                            filteredDishes = filteredDishes.OrderBy(filterBy => filterBy.Name);
                            break;

                        case Sorting.NameDesc:
                            filteredDishes = filteredDishes.OrderByDescending(filterBy => filterBy.Name);
                            break;

                        case Sorting.PriceAsc:
                            filteredDishes = filteredDishes.OrderBy(filterBy => filterBy.Price);
                            break;

                        case Sorting.PriceDesc:
                            filteredDishes = filteredDishes.OrderByDescending(filterBy => filterBy.Price);
                            break;

                        case Sorting.RatingAsc:
                            filteredDishes = filteredDishes.OrderBy(filterBy => filterBy.Rating);
                            break;

                        case Sorting.RatingDesc:
                            filteredDishes = filteredDishes.OrderByDescending(filterBy => filterBy.Rating);
                            break;
                    }

                    var dish = filteredDishes.Skip((currentPage - 1) * pageResult).Take(pageResult).ToList();

                    totalItems = dish.Count();
                    pageCount = (totalItems / pageResult) + 1;

                    dish.Select(dish => _mapper.Map<GetDishDto>(dish)).ToList();

                    var response = new ServiceResponses(dish, currentPage, totalItems, pageCount);
                    return response;
                }
            }

            throw new Exception("Bad request");
        }

 
 */