using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestApi.Models;
using TestApi.Wrappers;
using TestApi.Filters;
using TestApi.Helpers;
using TestApi.Services;

namespace TestApi.Controllers
{
    [Route("api/Ticket")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly TicketContext context;
        private readonly IUriService uriService;

        public TicketController(TicketContext context, IUriService service)
        {
            this.context = context;
            this.uriService = service;
        }

        // GET: api/Ticket
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetAllTickets(
            [FromQuery] PaginationFilter paginationFilter, 
            [FromQuery] TicketStatus status,
            [FromQuery] string ticketName)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(paginationFilter.pageNumber, paginationFilter.pageSize);
            var tickets = from t in context.Tickets
                    .Include(t=>t.assignee).Include(t=>t.owner).Include(t => t.category) select t;
            if(status != TicketStatus.None)
            {
                tickets =  tickets.Where(t => t.statusId == (int)status);
            }
            if(ticketName != null){
                tickets = tickets.Where(t => t.ticketName.Contains(ticketName));
            }
            
            var ticketList = await tickets
                            .Skip((validFilter.pageNumber-1) * validFilter.pageSize)
                            .Take(validFilter.pageSize).ToListAsync();

            foreach(Ticket t in ticketList){
                t.assignee.assignedTickets=null;
                t.owner.ownedTickets=null;
            }
            var totalRecords = await context.Tickets.CountAsync();
            var pagedResponse = PaginationHelper.CreatePagedReponse<Ticket>(ticketList,validFilter,totalRecords,uriService, route);
            return Ok(pagedResponse);
        }
    
        // GET: api/Ticket/{id}/owned
        [HttpGet("{uid}/owned")]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetOwnedTickets(
            [FromQuery] PaginationFilter paginationFilter, 
            [FromQuery] TicketStatus status,
            int uid
        )
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(paginationFilter.pageNumber, paginationFilter.pageSize);
            var tickets = from t in context.Tickets.Include(t => t.assignee) select t;
            if(status != TicketStatus.None)
            {
                tickets =  tickets.Where(t => t.statusId == (int)status);
            }
            var ticketList = await tickets
                            .Where(t => t.ownerId == uid)
                            .Skip((validFilter.pageNumber-1) * validFilter.pageSize)
                            .Take(validFilter.pageSize)
                            .ToListAsync();
            var totalRecords = await context.Tickets.CountAsync();
            var pagedResponse = PaginationHelper.CreatePagedReponse<Ticket>(ticketList,validFilter,totalRecords,uriService, route);
            return Ok(pagedResponse);
        }

        // GET: api/Ticket/{id}
        [HttpGet("{uid}/assigned")]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetAssignedTickets(
            [FromQuery] PaginationFilter paginationFilter, 
            [FromQuery] TicketStatus status,
            int uid)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(paginationFilter.pageNumber, paginationFilter.pageSize);
            var tickets = from t in context.Tickets.Include(t => t.owner) select t;
            if(status != TicketStatus.None)
            {
                tickets =  tickets.Where(t => t.statusId == (int)status);
            }
            var ticketList = await tickets
                            .Where(t => t.assigneeId == uid)
                            .Skip((validFilter.pageNumber-1) * validFilter.pageSize)
                            .Take(validFilter.pageSize)
                            .ToListAsync();
            var totalRecords = await context.Tickets.CountAsync();
            var pagedResponse = PaginationHelper.CreatePagedReponse<Ticket>(ticketList,validFilter,totalRecords,uriService, route);
            return Ok(pagedResponse);
        }

        // PUT: api/Ticket/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicket(int id, Ticket ticket)
        {
            if (id != ticket.ticketId)
            {
                return BadRequest();
            }

            context.Entry(ticket).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Ticket
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Response<Ticket>>> PostTicket(TicketRequest request)
        {
            Ticket ticket = new Ticket();
            Response<Ticket> response = new Response<Ticket>();
            
            bool valid = true;
            List<string> errors = new List<string>();
            
            //Ticket name cannot be empty
            if(request.ticketName != string.Empty){
                ticket.ticketName = request.ticketName;
            }else{
                errors.Add("Empty Name!");
                valid = false;
            }

            if(request.ticketName != string.Empty){
                ticket.ticketName = request.ticketName;
            }else{
                errors.Add("Empty Name!");
                valid = false;
            }

            ticket.description = request.description;
            ticket.assigneeId = request.assigneeId;
            ticket.ownerId = request.ownerId;

            if(request.categoryId > 0){
                ticket.categoryId = request.categoryId;
            }else{
                errors.Add("Empty Category!");
                valid = false;
            }

            response.errors = errors.ToArray();
            
            //Ticket created will always start with open status
            ticket.statusId = (int)TicketStatus.Open;

            //Date created follows server time
            ticket.dateCreated = DateTime.Today;

            if(valid){
                context.Tickets.Add(ticket);
                await context.SaveChangesAsync();
                response.data = ticket;
            }else{
                response.succeeded = false;
            }

            return response;
        }

        // DELETE: api/Ticket/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticket = await context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            context.Tickets.Remove(ticket);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool TicketExists(int id)
        {
            return context.Tickets.Any(e => e.ticketId == id);
        }
    }
}
