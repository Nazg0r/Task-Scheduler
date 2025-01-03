﻿using DataAccess.Data;
using DataAccess.Entities;
using Microsoft.Extensions.DependencyInjection;
using Tests.Integration.Factories;

namespace Tests.Integration.DataAccessTests
{
	public class UserRepositoryTests : IClassFixture<UserFactory>
	{
		private readonly UnitOfWork _unitOfWork;
		public UserRepositoryTests(UserFactory factory) =>
			_unitOfWork = new(factory.Services.GetRequiredService<DataContext>());

		[Theory]
		[InlineData("Oleksandr", "Shevchenko")]
		[InlineData("Dmytro", "Kovalchuk")]
		[InlineData("Andriy", "Grytsenko")]
		public async System.Threading.Tasks.Task GetByFullNameAsync_ReturnCorrectUser(string name, string surname)
		{
			//Arrange

			//Act
			User? dbUser = await _unitOfWork.UserRepository.GetByFullNameAsync(name, surname);

			//Assert
			Assert.NotNull(dbUser);
			Assert.Equal(name, dbUser.Name);
			Assert.Equal(surname, dbUser.Surname);
		}

		[Theory]
		[InlineData("Vika", "Savchenko")]
		[InlineData("Pavlo", "Bobrovich")]
		[InlineData("Den", "Tenou")]
		public async System.Threading.Tasks.Task AddAsync_CreateNewUser(string name, string surname)
		{
			//Arrange
			User user = new User()
			{
				Name = name,
				Surname = surname
			};

			//Act
			await _unitOfWork.UserRepository.AddAsync(user);
			await _unitOfWork.SaveAsync();
			User? dbUser = await _unitOfWork.UserRepository.GetByFullNameAsync(user.Name, user.Surname);

			//Assert
			Assert.NotNull(dbUser);
			Assert.Equal(name, dbUser.Name);
			Assert.Equal(surname, dbUser.Surname);
		}

		[Theory]
		[InlineData("Volodymyr", "Tkachenko", "Tyler", "Durden")]
		[InlineData("Kateryna", "Moroz", "Fedir", "Denchyk")]
		public async System.Threading.Tasks.Task Update_ChangeUserFields(string oldName, string oldSurname, string newName, string newSurname)
		{
			//Arrange
			User? dbUser = await _unitOfWork.UserRepository.GetByFullNameAsync(oldName, oldSurname);
			dbUser.Name = newName;
			dbUser.Surname = newSurname;

			//Act
			_unitOfWork.UserRepository.Update(dbUser);
			await _unitOfWork.SaveAsync();
			User? updatedUser = await _unitOfWork.UserRepository.GetByFullNameAsync(newName, newSurname);
			User? oldUser = await _unitOfWork.UserRepository.GetByFullNameAsync(oldName, oldSurname);


			//Asert
			Assert.NotNull(updatedUser);
			Assert.Null(oldUser);
			Assert.Equal(newName, updatedUser.Name);
			Assert.Equal(newSurname, updatedUser.Surname);
		}

		[Theory]
		[InlineData("Olha", "Sydenko")]
		[InlineData("Iryna", "Petrenko")]
		public async System.Threading.Tasks.Task Delete_RemoveUser(string name, string surname)
		{
			//Arange
			User? user = await _unitOfWork.UserRepository.GetByFullNameAsync(name, surname);

			//Act
			_unitOfWork.UserRepository.Delete(user);
			await _unitOfWork.SaveAsync();
			User? deletedeUser = await _unitOfWork.UserRepository.GetByFullNameAsync(name, surname);

			//Assert
			Assert.Null(deletedeUser);
		}
	}
}
