using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularTest.Models;
using AngularTest1.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AngularTest_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FundingsController : ControllerBase
    {
        #region FieldsAndConstructor
        private mvangularContext _dbContext;
        private IMapper _mapper;

        public FundingsController(mvangularContext mvangularContext,
            IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = mvangularContext;
        }
        #endregion
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                //var joinData = _dbContext.FeatureFunding.Join(_dbContext.FeatureFundinginterest, rdi => rdi.Id, c => c.FeatureFundingId,
                //    (rdi, c) => new { FeatureFunding = rdi, FeatureFundinginterest = c });
                //var featureFunding = joinData.Select(s=>s.FeatureFunding);
                //var featureFundingUser = joinData.Select(s => s.FeatureFundinginterest);
                var items = await _dbContext.FeatureFunding.ToListAsync();
                if (items != null)
                {
                    return Ok(_mapper.Map<IEnumerable<FeatureFundingViewModel>>(items));
                }
                return NotFound("There are no items in database");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //In Production Mode Use this
                //return BadRequest($"Something bad happened!");

                return BadRequest($"Something bad happened! Exception is {e.Message}");
            }
        }
        [HttpGet("{id:min(1)}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var item = await _dbContext.FeatureFunding.FirstOrDefaultAsync(d => d.Id == id); ;
                if (item == null) return NotFound($"item with Id {id} not found");
                return Ok(_mapper.Map<FeatureFundingViewModel>(item));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //In Production Mode Use this And log the Error in Auditlog
                //return BadRequest($"Something bad happened!");
                return BadRequest($"Something bad happened! Exception is {e.Message}");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FundingUserViewModel model)
        {
            try
            {
                var entity = _mapper.Map<FundingUserViewModel>(model);
                if (entity != null)
                {
                    _dbContext.Add(entity);
                    await _dbContext.SaveChangesAsync();
                    
                        return Created("", _mapper.Map<FundingUserViewModel>(entity));
                    
                }
                return BadRequest("Something bad happened!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //In Production Mode Use this And log the Error in Auditlog
                //return BadRequest($"Something bad happened!");
                return BadRequest($"Something bad happened! Exception is {e.Message}");
            }
        }
    }
}