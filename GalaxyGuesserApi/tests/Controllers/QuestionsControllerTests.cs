using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using GalaxyGuesserApi.Controllers;
using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Services.Interfaces;

namespace GalaxyGuesserApi.Tests.Controllers
{
    public class QuestionsControllerTests
    {
       private readonly Mock<IQuestionService> _mockQuestionService;
        private readonly QuestionsController _controller;

        public QuestionsControllerTests()
        {
            _mockQuestionService = new Mock<IQuestionService>();
            _controller = new QuestionsController(_mockQuestionService.Object);
        }

        [Fact]
        public async Task AskQuestion_WithValidSessionId_ReturnsQuestion()
        {
           var expectedQuestionResponse = new QuestionResponse 
            { 
                QuestionId = 1, 
                QuestionText = "Test question",
            };
            
            _mockQuestionService.Setup(x => x.GetNextQuestionForSessionAsync(It.IsAny<int>()))
                .ReturnsAsync(expectedQuestionResponse);

            var result = await _controller.AskQuestion(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedQuestion = Assert.IsType<QuestionResponse>(okResult.Value);
            Assert.Equal(expectedQuestionResponse.QuestionId, returnedQuestion.QuestionId);
        }

        [Fact]
        public async Task GetOptions_WithValidQuestionId_ReturnsOptions()
        {
           var expectedOptions = new List<OptionResponse>
            {
                new OptionResponse { AnswerId = 1, Text = "Option 1" },
                new OptionResponse { AnswerId = 2, Text = "Option 2" }
            };

            _mockQuestionService.Setup(x => x.GetOptionsByQuestionIdAsync(It.IsAny<int>()))
                .ReturnsAsync(expectedOptions);

            var result = await _controller.GetOptions(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedOptions = Assert.IsType<List<OptionResponse>>(okResult.Value);
            Assert.Equal(2, returnedOptions.Count);
        }

        [Fact]
        public async Task GetOptions_WithInvalidQuestionId_ReturnsNotFound()
        {
              _mockQuestionService.Setup(x => x.GetOptionsByQuestionIdAsync(It.IsAny<int>()))
                .ReturnsAsync((List<OptionResponse>)null);

            var result = await _controller.GetOptions(999);

            Assert.IsType<NotFoundObjectResult>(result);

        }

        [Fact]
        public async Task GetCorrectAnswer_WithValidQuestionId_ReturnsAnswer()
        {
            var expectedAnswer = new AnswerResponse { AnswerId = 1, Text = "Correct answer" };
            _mockQuestionService.Setup(x => x.GetCorrectAnswerAsync(It.IsAny<int>()))
                .ReturnsAsync(expectedAnswer);

            var result = await _controller.GetCorrectAnswer(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedAnswer = Assert.IsType<AnswerResponse>(okResult.Value);
            Assert.Equal(expectedAnswer.AnswerId, returnedAnswer.AnswerId);
            Assert.Equal(expectedAnswer.AnswerId, returnedAnswer.AnswerId);
        }

        [Fact]
        public async Task GetCorrectAnswer_WithInvalidQuestionId_ReturnsNotFound()
        {
            _mockQuestionService.Setup(x => x.GetCorrectAnswerAsync(It.IsAny<int>()))
                .ReturnsAsync((AnswerResponse)null);

            var result = await _controller.GetCorrectAnswer(999);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}