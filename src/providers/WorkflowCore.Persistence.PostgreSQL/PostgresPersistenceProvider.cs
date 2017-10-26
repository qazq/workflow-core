﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkflowCore.Persistence.EntityFramework.Models;
using WorkflowCore.Persistence.EntityFramework.Services;

namespace WorkflowCore.Persistence.PostgreSQL
{
    public class PostgresPersistenceProvider : EntityFrameworkPersistenceProvider
    {
        private readonly string _connectionString;

        public PostgresPersistenceProvider(string connectionString, bool canCreateDB, bool canMigrateDB)
            :base(canCreateDB, canMigrateDB)
        {   
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql(_connectionString);
        }

        protected override void ConfigureSubscriptionStorage(EntityTypeBuilder<PersistedSubscription> builder)
        {
            builder.ToTable("Subscription", "wfc");
            builder.Property(x => x.PersistenceId).ValueGeneratedOnAdd();
        }

        protected override void ConfigureWorkflowStorage(EntityTypeBuilder<PersistedWorkflow> builder)
        {
            builder.ToTable("Workflow", "wfc");
            builder.Property(x => x.PersistenceId).ValueGeneratedOnAdd();
        }
                
        protected override void ConfigureExecutionPointerStorage(EntityTypeBuilder<PersistedExecutionPointer> builder)
        {
            builder.ToTable("ExecutionPointer", "wfc");
            builder.Property(x => x.PersistenceId).ValueGeneratedOnAdd();
        }

        protected override void ConfigureExecutionErrorStorage(EntityTypeBuilder<PersistedExecutionError> builder)
        {
            builder.ToTable("ExecutionError", "wfc");
            builder.Property(x => x.PersistenceId).ValueGeneratedOnAdd();
        }

        protected override void ConfigureExetensionAttributeStorage(EntityTypeBuilder<PersistedExtensionAttribute> builder)
        {
            builder.ToTable("ExtensionAttribute", "wfc");
            builder.Property(x => x.PersistenceId).ValueGeneratedOnAdd();
        }

        protected override void ConfigureEventStorage(EntityTypeBuilder<PersistedEvent> builder)
        {
            builder.ToTable("Event", "wfc");
            builder.Property(x => x.PersistenceId).ValueGeneratedOnAdd();
        }
    }
}

