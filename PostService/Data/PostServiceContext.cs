﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PostService.Data
{
    public class PostServiceContext: DbContext
    {
        public PostServiceContext(DbContextOptions<PostServiceContext> options)
            : base(options)
        {}

        public DbSet<PostService.Entities.Post> Post { get; set; }
        public DbSet<PostService.Entities.User> Users { get; set; }

    }
}
