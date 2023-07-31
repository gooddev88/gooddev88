﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Robot.Data.GADB.TT;

namespace Robot.Data.GADB.TT
{
    public partial class GAEntities
    {
        private GAEntitiesProcedures _procedures;

        public GAEntitiesProcedures Procedures
        {
            get
            {
                if (_procedures is null) _procedures = new GAEntitiesProcedures(this);
                return _procedures;
            }
            set
            {
                _procedures = value;
            }
        }

        public GAEntitiesProcedures GetProcedures()
        {
            return Procedures;
        }
    }

    public partial class GAEntitiesProcedures
    {
        private readonly GAEntities _context;

        public GAEntitiesProcedures(GAEntities context)
        {
            _context = context;
        }

        public virtual async Task<SP_LIST_USERINCOMPANYResult[]> SP_LIST_USERINCOMPANYAsync(string Username, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "Username",
                    Size = 100,
                    Value = Username ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SP_LIST_USERINCOMPANYResult>("EXEC @returnValue = [dbo].[SP_LIST_USERINCOMPANY] @Username", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }
    }
}