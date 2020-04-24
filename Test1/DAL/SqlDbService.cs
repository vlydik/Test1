using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Test1.Models;

namespace Test1.Services
{
    public class SqlDbService : IDbService
    {
        public const string con = "Data Source=db-mssql;Initial Catalog=s19183;Integrated Security=True";

        public SqlDbService()
        {
        }

        
        public bool DeleteProject(string id)
        {
            using (var connection = new SqlConnection(con))
            {
                using(var command = new SqlCommand())
                {
                    command.Connection = connection;
                    connection.Open();

                    command.CommandText = "SELECT IdProject FROM Project WHERE IdProject = @IdProject";
                    command.Parameters.AddWithValue("@IdProject", id);
                    var reader = command.ExecuteReader();
                    if (!reader.Read())
                    {
                        reader.Close();
                        return false;
                    }
                    reader.Close();

                    var tran = connection.BeginTransaction();
                    command.Transaction = tran;

                   

                    command.CommandText = "DELETE FROM Task WHERE IdTeam = @IdTeam";
                    command.ExecuteNonQuery();

                    command.CommandText = "DELETE FROM Project WHERE IdTeam = @IdTeam";
                    command.ExecuteNonQuery();

                    tran.Commit();
                }
            }
            return true;
        }
        

        

        public TeamMember GetTeamMember(string id)
        {
            TeamMember tm = new TeamMember();
            tm.Tasks = new List<TeamMember>();

            using(var connection = new SqlConnection(con))
            {
                using(var command = new SqlCommand())
                {
                    command.Connection = connection;
                    connection.Open();

                    command.CommandText = "SELECT * FROM TeamMember WHERE IdTeamMember = @IdTeamMember";
                    command.Parameters.AddWithValue("@IdTeamMember", id);
                    var reader = command.ExecuteReader();
                    if (!reader.Read())
                    {
                        reader.Close();
                        return null;
                    }
                    var TeamMember = new TeamMember();
                    TeamMember.IdTeamMember = reader[0] as string;
                    TeamMember.FirstName = reader[1] as string;
                    TeamMember.LastName = reader[2] as string;
                    TeamMember.Email = reader[3] as string;
                    
                    command.Parameters.Clear();
                    reader.Close();
                    Console.WriteLine("Created new member");

                    command.CommandText =
                    "SELECT IdTask,Task.Name,Task.Description,Task.Deadline,P.Name,TT.Name FROM Task JOIN TaskType TT ON Task.IdTaskType = TT.IdTaskType JOIN Project P ON Task.IdProject = P.IdProject WHERE IdAssignedTo = @id;";
                    command.Parameters.AddWithValue("@IdTeamMember", id);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        tm.IdTeamMember = reader[0] as string;
                        tm.FirstName = reader[1] as string;
                        tm.LastName = reader[2] as string;
                        tm.Email = reader[3] as string
                        
                    }
                    command.Parameters.Clear();
                    reader.Close();

                    command.CommandText = "SELECT IdTask,Task.Name,Task.Description,Task.Deadline,P.Name,TT.Name FROM Task JOIN TaskType TT ON Task.IdTaskType = TT.IdTaskType JOIN Project P ON Task.IdProject = P.IdProject WHERE IdCreator = @id ORDER BY Task.Deadline desc;";
                    command.Parameters.AddWithValue("@IdTeamMember", id);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        tm.IdTeamMember = reader[0] as string;
                        tm.FirstName = reader[1] as string;
                        tm.LastName = reader[2] as string;
                        tm.Email = reader[3] as string
                    }
                }
                return tm;
            }

     
        }
    }
}
