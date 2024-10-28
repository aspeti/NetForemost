using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;


namespace BlogApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BlogEntriesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public BlogEntriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets list of BlogEntry
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<BlogEntry>>> GetAll()
        {
            try
            {
                var entity = await _unitOfWork.BlogEntry                   
                    .GetAllAsync(e => e.BetDeleted == false);
                return Ok(entity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets a BlogEntry by Id
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BlogEntry>> GetById(int id)
        {
            try
            {

                var entity = await _unitOfWork.BlogEntry.GetByIdAsync(id);

                if (entity == null)
                    return NotFound($"Blog entry with ID {id} not found or has been deleted.");

                return Ok(entity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Create a BlogEntry
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BlogEntry>> Create( int userId , [FromBody] BlogEntry blogEntry)
        {
            try
            {

                if (blogEntry == null || userId ==0)
                    return BadRequest("User data is null.");

                var user = await _unitOfWork.User.GetByIdAsync(userId);

                var category = await _unitOfWork.Categories.GetByIdAsync(blogEntry.BetCatId);

            
                if (user != null && category !=null)
                {
                    blogEntry.Users.Add(user);
                    blogEntry.BetCat = category;
                }
                else
                {
                    return NotFound("User not found.");
                }

                await _unitOfWork.BlogEntry.AddAsync(blogEntry);
                await _unitOfWork.CompleteAsync();

                return CreatedAtAction(nameof(GetById), new { id = blogEntry.BetId }, blogEntry);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update a BlogEntry
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] BlogEntry blogEntry)
        {
            try
            {
                if (blogEntry == null || blogEntry.BetId != id)
                    return BadRequest("Invalid blogEntry data.");

                var entity = await _unitOfWork.BlogEntry.GetByIdAsync(id);
                if (entity == null)
                    return NotFound($"BlogEntry with ID {id} not found.");


                entity.BetTitle = blogEntry.BetTitle;
                entity.BetContent = blogEntry.BetContent;
                entity.BetAutor = blogEntry.BetAutor;
                entity.BetPublicationDate = blogEntry.BetPublicationDate;

                _unitOfWork.BlogEntry.Update(entity);
                await _unitOfWork.CompleteAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete a BlogEntry by Id
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var entity = await _unitOfWork.BlogEntry.GetByIdAsync(id);
                if (entity == null)
                    return NotFound($"BlogEntry with ID {id} not found.");

                entity.BetDeleted = true;
                _unitOfWork.BlogEntry.Update(entity);
                await _unitOfWork.CompleteAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
