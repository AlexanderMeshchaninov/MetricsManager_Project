using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Dapper;
using MetricsManager.Models;

namespace MetricsManager.DAL
{
    public interface IAgentsRepository : IAgentsRepository<AgentInfo>
    {
    }

    public class AgentsRepository : IAgentsRepository
    {
        private readonly IDbConnection _dbConnection;

        public AgentsRepository(IDbConnection dbConnection)
        {
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());

            _dbConnection = dbConnection;
        }

        public void Create(AgentInfo item)
        {
            using (var connection = new SQLiteConnection(_dbConnection.AddConnectionDb()))
            {
                connection.Execute("INSERT INTO agents(AgentId, AgentAddress, Status) VALUES(@AgentId, @AgentAddress, @Status)",
                    new
                    {
                        AgentId = item.AgentId,
                        AgentAddress = item.AgentAddress,
                        Status = item.Status,
                    });
            }
        }

        public List<AgentInfo> Read()
        {
            using (var connection = new SQLiteConnection(_dbConnection.AddConnectionDb()))
            {
                return connection.Query<AgentInfo>("SELECT * FROM agents").ToList();
            }
        }

        public void Update(int agentId, int status)
        {
            using (var connection = new SQLiteConnection(_dbConnection.AddConnectionDb()))
            { 
                connection.Execute($"UPDATE agents SET Status={status} WHERE AgentId=@AgentId",
                    new
                    {
                        AgentId = agentId,
                    });
            }
        }
    }
}