using Dapper;
using HMS.Interface;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System;
using System.Linq;
using System.Data.SqlClient;


namespace HMS.Services
{
    public class DapperService : IDapperService
    {
        private readonly IConfiguration _config;
        public DapperService(IConfiguration config)
        {
            _config = config;
        }
        public void Dispose()
        {
        }
        public T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text, string connectionName = null)
        {
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(connectionName));
            return db.Query<T>(sp, parms, commandType: commandType).FirstOrDefault();
        }
        public List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure, string connectionName = null)
        {
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(connectionName));
            return db.Query<T>(sp, parms, commandType: commandType).ToList();
        }
        public DbConnection GetDbconnection(string connectionName)
        {
            return new SqlConnection(_config.GetConnectionString(connectionName));
        }
        public T Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure, string connectionName = null)
        {
            T result;
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(connectionName));
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                using var tran = db.BeginTransaction();
                try
                {
                    result = db.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }
            return result;
        }
        public T Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure, string connectionName = null)
        {
            T result;
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(connectionName));
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                using var tran = db.BeginTransaction();
                try
                {
                    result = db.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }
            return result;
        }

    }
}
