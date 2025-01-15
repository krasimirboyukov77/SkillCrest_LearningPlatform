# Teacher-Student Course Platform

### !IMPORTANT!  
**The project does not start on the first launch!**  
You need to launch it again to start. This issue occurs due to a foreign key problem during the first initialization.

A web application where teachers can create courses, and students can enroll in them to learn and track their progress. Perfect for managing educational content and student performance effectively.

## Table of Contents
- [Key Features](#key-features)
  - [Identity](#identity)
  - [Courses](#courses)
  - [Admin Features](#admin)
- [Project Structure and Architecture](#project-structure-and-architecture)
  - [Back-End](#back-end)
  - [Common Folder](#common-folder)
  - [Data Folder](#data-folder)
  - [Services Folder](#services-folder)
  - [Test](#test)
  - [ViewModels](#viewmodels)
  - [Web](#web)


The application supports three types of users: Student, Teacher, and Admin.
Students and Teachers have separate registration pages.
The app is pre-configured with two seeded users: an Admin and a Teacher. Their credentials are stored in the appsettings.json file.
### Admin Credentials:
- **Email**: pesho123@abv.bg  
- **Password**: Pesho123#

### Teacher Credentials:
- **Email**: teacher123@abv.bg  
- **Password**: Teacher123#

### Home Page
![home page](https://github.com/user-attachments/assets/2d12f9d0-6bb6-45f7-a09a-ce6a6806d61f)
### Courses Preview
![after login](https://github.com/user-attachments/assets/368ea783-1353-4ae0-bca8-ff24bc332756)
### Course
![course preview](https://github.com/user-attachments/assets/caf2e0bd-5838-459a-9206-db8b46d15515)
### Lesson
![lesson preview](https://github.com/user-attachments/assets/38a6fdba-f382-4e7b-bf59-001a2a25f712)
### Statistics Panel
![statistics preview](https://github.com/user-attachments/assets/9cb391df-e2b6-48c2-a588-8ba0daefdc8b)

## Key Features

### Identity
- Users can register as either a **Teacher** or a **Student**, with separate registration pages for each.  
  - **Students**: Register with a username, email, password, and the name of their school.  
  - **Teachers**: Provide the same details as students, along with a biography to introduce themselves.

### Courses

1. **Teachers**:  
   - Create and manage courses, lessons, and quizzes.  
   - Grade student homework and provide comments or feedback.

2. **Students**:  
   - Enroll in courses to access lessons and quizzes.  
   - Track their progress in enrolled courses.  
   - Upload submissions for lessons as required.


 ### Admin:  
  - Full platform control:  
    - Add, edit, or delete courses and lessons.  
    - Manage users: update roles, remove users, and oversee platform activity.

## Project Structure and Architecture
![database diagram](https://github.com/user-attachments/assets/ed87b4f4-e720-4605-98da-51e4bee4bacf)

### Back-End
![back end](https://github.com/user-attachments/assets/01d13a98-68a1-4e80-862f-c8c9b298bb63)

Let's start from the top.
### Common Folder
![common folder](https://github.com/user-attachments/assets/52e9c22c-ee45-4c88-8e07-54e7e7245a8f)

All validation constants are separated in different folders for the entities.
Error messages are for the whole application.

### Data Folder
![data folder](https://github.com/user-attachments/assets/747de653-1b20-43af-ae3e-8878197e7b31)

The Data folder contains the following key components:

### Data
- **Migrations**: Handles database schema changes and version control.  
- **Seeds**: Provides initial data for courses and lessons to set up the application.  
- **ApplicationDbContext**: Includes a global filter to exclude deleted objects from queries automatically.

### Models
![models updated](https://github.com/user-attachments/assets/fcdaa255-9e01-4a22-b3b9-0ed524210aeb)
- **Models**: Models used in the application.

### Services Folder

![services folder](https://github.com/user-attachments/assets/969c3d01-169e-4ad7-870a-b26ad99c03bf)

#### **Infrastructure**

- **ApplicationBuilderExtensions**: Contains the following extensions:  
  - **SeedAdministrator**  
  - **SeedTeacher**  
  - **CreateAdminUserAsync**  
  - **CreateTeacherAsync**  

- **DatabaseInitializer**: Checks if the database exists and applies all pending migrations.  

- **Repositories**: Houses the primary repository implementation for the application.

### Services
![all services](https://github.com/user-attachments/assets/53ccc10e-9b29-4836-aeea-21848342e354)

- **Extensions**:  
  - **RegisterAppServices**: Registers all the services used in the application.  
  - **Seed**: Automatically seeds the database with initial courses and lessons.  

- **Interfaces**: Contains interfaces defining the contracts for the services.  

- **Services**: Implements the logic for all application services.

### Test

**This feature is not implemented yet!**

### ViewModels

![viewmodels](https://github.com/user-attachments/assets/0fe86b16-01f4-49ae-bb16-96a95b562622)

Contains all the ViewModels used across the application for data representation and binding.

### Web

**The main project of the application.**

- **Areas**:  
  ![Areas](https://github.com/user-attachments/assets/b3372efb-a2df-40f6-a1d5-8d7dbcae16f3)  
  Includes an area specifically for the admin to manage the application.
