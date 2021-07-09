using System;
using Xunit;
using DiffProject.Application.CommandHandlers;
using DiffProject.Application.Commands;
using DiffProject.Application.Enums;

namespace DiffProject.Tests.UnitTests
{
    
    public class SettingDataTests
    {
        [Fact]
        public async void SetNewComparisonRightData()
        {
            SetDataCommandHandler commandHandler = new SetDataCommandHandler();
            var newId = await commandHandler.ExecuteAsync(new SetDataCommand
                                                {
                                                    ComparisonID = Guid.NewGuid(),
                                                    ComparisonSide = ComparisonSideEnum.Left,
                                                    Base64BinaryData = "a"
                                                });
            
                    
        }
    }
}
