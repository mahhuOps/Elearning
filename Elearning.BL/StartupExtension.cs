using Core.Implements;
using Core.Interfaces;
using DatabaseService;
using Elearning.BL.Implements;
using Elearning.BL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using FirebaseService;

namespace Elearning.BL
{
    public static class StartupExtension
    {
        public static void UseBL(this IServiceCollection services)
        {
            services.UseFirebaseService();
            services.AddTransient<IChapterBL, ChapterBL>();
            services.AddTransient<ICourseBL, CourseBL>();
            services.AddTransient<ICoursePackageBL, CoursePackageBL>();
            services.AddTransient<ICoursePackageDetailBL, CoursePackageDetailBL>();
            services.AddTransient<ICourseSessionBL, CourseSessionBL>();
            services.AddTransient<ILessonBL, LessonBL>();
            services.AddTransient<IServiceBL, ServiceBL>();
            services.AddTransient<IUserBL, UserBL>();
            services.AddTransient<IPromotionBL, PromotionBL>();
            services.AddTransient<ICheckoutBL, CheckoutBL>();
        }
    }
}
