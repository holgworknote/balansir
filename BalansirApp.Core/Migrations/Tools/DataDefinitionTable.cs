﻿using BalansirApp.Core.Common.DataAccess;
using BalansirApp.Core.Migrations.Tools.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BalansirApp.Core.Migrations.Tools
{
    class DataDefinitionTable<TTable> : IDataDefinitionTable<TTable>
    {
        private readonly SQLiteConnection _db;

        public List<Exception> Exceptions { get; } = new List<Exception>();

        // CTOR
        public DataDefinitionTable(SQLiteConnection db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public IDataDefinitionTable<TTable> AddColumn<TColumn>(
            Expression<Func<TTable, TColumn>> propertyLambda,
            SQLiteColumnType columnType,
            bool isPrimaryKey = false,
            bool isAutoincrement = false,
            bool isNotNull = false)
        {
            Func<Exception> func = () => _db.AddColumn(
                propertyLambda,
                columnType,
                isPrimaryKey,
                isAutoincrement,
                isNotNull
            );
            return this.BaseMethod(func);
        }

        public IDataDefinitionTable<TTable> AddIndex<TColumn>(
            Expression<Func<TTable, TColumn>> propertyLambda,
            bool isUnique = false)
        {
            return this.BaseMethod(() => _db.AddIndex(propertyLambda, isUnique));
        }

        DataDefinitionTable<TTable> BaseMethod(Func<Exception> func)
        {
            var newException = func();

            if (newException != null)
                this.Exceptions.Add(newException);

            return this;
        }
    }
}
