﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Pages.Departments
{
    public class Index : PageModel
    {
        private readonly IMediator _mediator;

        public Index(IMediator mediator) => _mediator = mediator;

        public List<Model> Data { get; private set; }

        public async Task OnGetAsync()
            => Data = await _mediator.Send(new Query());

        public class Query : IRequest<List<Model>>
        {
        }

        public class Model
        {
            public string Name { get; set; }

            public decimal Budget { get; set; }

            public DateTime StartDate { get; set; }

            public int Id { get; set; }

            public string AdministratorFullName { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile() => CreateMap<Department, Model>();
        }

        public class QueryHandler : AsyncRequestHandler<Query, List<Model>>
        {
            private readonly SchoolContext _context;

            public QueryHandler(SchoolContext context) => _context = context;

            protected override async Task<List<Model>> HandleCore(Query message)
            {
                var projectTo = _context.Departments
                    .ProjectTo<Model>();
                return await projectTo.ToListAsync();
            }
        }
    }
}