using MathDotSqrt.Sqrt3D.Util.IO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.Util.Query {
	class MySQL {
		private MySqlConnection connection;
		private string server;
		private string database;
		private string uid;
		private string password;

		//Constructor
		public MySQL() {
			Initialize();
		}

		//Initialize values
		private void Initialize() {
			server = "officialhosting.site.nfoservers.com";
			database = "officialhosting_ludumdare37";
			uid = "officialhosting";
			password = "&fXnbLhPtAq2YGJU";
			string connectionString;
			connectionString = "SERVER=" + server + ";" + "DATABASE=" +
			database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

			connection = new MySqlConnection(connectionString);
		}

		//open connection to database
		private bool OpenConnection() {
			try {
				connection.Open();
				return true;
			} catch (MySqlException) {
				return false;
			}
		}

		//Close connection
		private bool CloseConnection() {
			try {
				connection.Close();
				return true;
			} catch (MySqlException) {
				return false;
			}
		}

		//Insert statement
		public void Insert(int playCount, int playTime, DateTime starttime, DateTime closetime, int completedgame) {
			string query = "INSERT INTO data (playcount, playtime, starttime, closetime, completedgame) VALUES(" + playCount.ToString() + ',' + playTime.ToString() + ','
				+ starttime.ToString() + ',' + closetime.ToString() + ',' + completedgame.ToString() + ")";

			//open connection
			if (this.OpenConnection()) {
				MySqlCommand cmd = new MySqlCommand(query, connection);

				cmd.ExecuteNonQuery();

				this.CloseConnection();
			}
		}

		//Update statement
		public void Update() {
			string query = "UPDATE tableinfo SET name='Joe', age='22' WHERE name='John Smith'"; //exam

			if (this.OpenConnection()) {
				MySqlCommand cmd = new MySqlCommand();

				cmd.CommandText = query;

				cmd.Connection = connection;

				cmd.ExecuteNonQuery();

				this.CloseConnection();
			}
		}

		//Delete statement
		public void Delete() {
			string query = "DELETE FROM tableinfo WHERE name='ex...'";

			if (this.OpenConnection() == true) {
				MySqlCommand cmd = new MySqlCommand(query, connection);
				cmd.ExecuteNonQuery();
				this.CloseConnection();
			}
		}

		//Select statement
		public List<string>[] Select() {
			string query = "SELECT * FROM tableinfo";

			List<string>[] list = new List<string>[5];
			list[0] = new List<string>();
			list[1] = new List<string>();
			list[2] = new List<string>();
			list[3] = new List<string>();
			list[4] = new List<string>();

			if (this.OpenConnection()) {
				MySqlCommand cmd = new MySqlCommand(query, connection);
				MySqlDataReader dataReader = cmd.ExecuteReader();

				while (dataReader.Read()) {
					list[0].Add(dataReader["id"] + "");
					list[1].Add(dataReader["playcount"] + "");
					list[2].Add(dataReader["playtime"] + "");
					list[3].Add(dataReader["starttime"] + "");
					list[2].Add(dataReader["closetime"] + "");
					list[2].Add(dataReader["completedgame"] + "");
				}

				dataReader.Close();

				this.CloseConnection();

				return list;
			} else {
				return list;
			}
		}

		//Count statement
		public int Count() {
			string query = "SELECT Count(*) FROM tableinfo";
			int Count = -1;

			if (this.OpenConnection()) {
				MySqlCommand cmd = new MySqlCommand(query, connection);

				Count = int.Parse(cmd.ExecuteScalar() + "");

				this.CloseConnection();

				return Count;
			} else {
				return Count;
			}
		}
	}
}