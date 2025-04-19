# System Patterns

The system follows a layered architecture with the following layers:

*   **API Layer:** Exposes the API endpoints for managing the Elearning platform.
*   **Business Logic Layer:** Contains the business logic for the Elearning platform.
*   **Data Access Layer:** Provides access to the database.
*   **Service Layer:** Provides access to external services such as Firebase and EmailService.

The system uses the following design patterns:

*   **Dependency Injection:** Used to inject dependencies into the different layers.
*   **Repository Pattern:** Used to abstract the data access layer.
*   **Unit of Work Pattern:** Used to manage database transactions.

The project uses the following data models:

*   **Chapter:** Represents a chapter in a course.
*   **Course:** Represents a course.
*   **CoursePackage:** Represents a package of courses.
*   **CoursePackageDetail:** Represents the details of a course package.
*   **CourseSession:** Represents a session in a course.
*   **Lesson:** Represents a lesson in a chapter.about the data models, the business logic, the API endpoints, the database structure
*   **Service:** Represents a service offered in a course package.
*   **User:** Represents a user of the platform.

The project uses the following business logic classes:

*   **ChapterBL:** Provides business logic for chapters.
*   **CourseBL:** Provides business logic for courses.
*   **CoursePackageBL:** Provides business logic for course packages.
*   **CoursePackageDetailBL:** Provides business logic for course package details.
*   **CourseSessionBL:** Provides business logic for course sessions.
*   **LessonBL:** Provides business logic for lessons.
*   **ServiceBL:** Provides business logic for services.
*   **UserBL:** Provides business logic for users.

All business logic classes inherit from `BaseBL` and implement a corresponding interface. They all take `IDatabaseService` as a dependency.

The project uses the following API endpoints:

*   **ChaptersController:** Provides API endpoints for managing chapters.
*   **CoursePackageDetailsController:** Provides API endpoints for managing course package details.
*   **CoursePackagesController:** Provides API endpoints for managing course packages.
*   **CoursesController:** Provides API endpoints for managing courses.
*   **CourseSessionsController:** Provides API endpoints for managing course sessions.
*   **LessonsController:** Provides API endpoints for managing lessons.
*   **ServicesController:** Provides API endpoints for managing services.
*   **UsersController:** Provides API endpoints for managing users.

All controllers inherit from `BaseController` and inject a corresponding business logic interface.

The project uses a MySQL database. The `DatabaseService` class uses Dapper to interact with the database. The connection string is retrieved from the configuration.
