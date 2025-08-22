using AutoMapper;
using bookstore.Models;
using bookstore.ViewModels.Books;

namespace bookstore.Mapper
{
    public class MappingConfig: Profile
    {
        public MappingConfig()
        {
            CreateMap<AddBookVM, Book>().ReverseMap();
            CreateMap<BookCardVM, Book>().ReverseMap();
            CreateMap<BookDetailsVM, Book>().ReverseMap();
        }
    }
}
