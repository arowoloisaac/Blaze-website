using Arowolo_Delivery_Project.Dtos.DishDto;

namespace Arowolo_Delivery_Project.Models
{
    public class ServiceResponses
    {
        public List<GetDishDto> Dishes { get; set; } = new List<GetDishDto>();
        public PageInfo Pagination { get; set; }

        public ServiceResponses(List<GetDishDto> dish, int page, int total, int count)
        {
            this.Dishes = dish;

            //this.pagination = new PageInfo(page, total, size);
            this.Pagination = new PageInfo
            {
                Count = count,
                Current = page,
                Size = total
            };
        }
    }


    public class PageInfo
    {
        public int Size { get; set; }// number of item in the particular page
        public int Current { get; set; }
        public int Count { get; set; } // number of pages

        public PageInfo() { }

        public PageInfo(int size, int current, int count)
        {
            this.Size = size;
            this.Current = current;
            this.Count = count;
        }
    }
}
