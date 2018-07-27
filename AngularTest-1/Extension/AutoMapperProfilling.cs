using AngularTest.Models;
using AngularTest1.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularTest1.Extension
{
    public class AutoMapperProfilling : Profile
    {
        public AutoMapperProfilling()
        {
            CreateMap<FeatureFunding, FeatureFundingViewModel>().ReverseMap();
        }  
    }
}
