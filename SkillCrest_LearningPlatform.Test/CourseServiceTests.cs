using Moq;
using SkillCrest_LearningPlatform.Data.Data.Models;
using SkillCrest_LearningPlatform.Data.Models;
using SkillCrest_LearningPlatform.Data.Models.QuizModels;
using SkillCrest_LearningPlatform.Infrastructure.Repositories.Contracts;

namespace SkillCrest_LearningPlatform.Test
{
    public class Tests
    {
        private Mock<IRepository<Course>> courseRepository;
        private Mock<IRepository<Lesson>> lessonRepository;
        private Mock<IRepository<Quiz>> quizRepository;
        private Mock<IRepository<UserCourse>> userCourseRepository;
        private Mock<IRepository<QuizSubmission>> quizSubmissionRepository;

        [SetUp]
        public void Setup()
        {
            this.courseRepository = new Mock<IRepository<Course>>();
            this.lessonRepository = new Mock<IRepository<Lesson>>();
            this.quizRepository = new Mock<IRepository<Quiz>>();
            this.quizSubmissionRepository = new Mock<IRepository<QuizSubmission>>();

        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}