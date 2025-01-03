﻿using Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
	[Table("tasks")]
	public class Task : BaseEntity
	{
		[Column("title")] public string Title { get; set; }
		[Column("description")] public string Description { get; set; }
		[Column("priority")] public string Priority { get; set; }
		[Column("state")] public string State { get; set; }
		[Column("time_to_finish")] public string TimeToFinish { get; set; }
		[Column("creation_date")] public string CreationDate { get; set; }
		public virtual ICollection<Employee> Employees { get; set; }

		public override bool Equals(object? obj)
		{
			return obj is Task task &&
			task.Id == Id &&
			task.Title == Title &&
			task.Description == Description &&
			task.Priority == Priority &&
			task.State == State &&
			task.TimeToFinish == TimeToFinish &&
			task.CreationDate == CreationDate &&
			task.Employees.Equals(Employees);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Id);
		}
	}
}
