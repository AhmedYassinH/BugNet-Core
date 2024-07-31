

# Software Requirements Specification - BugNet Core

## Table of Contents

1. [Introduction](#1-introduction)
   1.1 [Purpose](#11-purpose)
   1.2 [Scope](#12-scope)
   1.3 [Definitions and Acronyms](#13-definitions-and-acronyms)

2. [Functional Requirements](#2-functional-requirements)
   2.1 [User Management](#21-user-management)
   2.2 [Project Management](#22-project-management)
   2.3 [Bug Management](#23-bug-management)
   2.4 [Comment Management](#24-comment-management)
   2.5 [Notification Management](#25-notification-management)
   2.6 [Live Support Management](#26-live-support-management)
   2.7 [Chat Room Management](#27-chat-room-management)

3. [Non-Functional Requirements](#3-non-functional-requirements)
   3.1 [Performance Requirements](#31-performance-requirements)
   3.2 [Security Requirements](#32-security-requirements)
   3.3 [Scalability Requirements](#33-scalability-requirements)
   3.4 [Reliability Requirements](#34-reliability-requirements)
   3.5 [Usability Requirements](#35-usability-requirements)
   3.6 [Compatibility Requirements](#36-compatibility-requirements)

4. [System Architecture](#4-system-architecture)
   4.1 [High-Level Architecture](#41-high-level-architecture)
   4.2 [Database Design](#42-database-design)
   4.3 [API Design](#43-api-design)

5. [Data Requirements](#5-data-requirements)
   5.1 [Data Entities](#51-data-entities)
   5.2 [Data Retention](#52-data-retention)
   5.3 [Data Backup](#53-data-backup)

6. [External Interfaces](#6-external-interfaces
)
   6.1 [User Interfaces](#61-user-interfaces)
   6.2 [Hardware Interfaces](#62-hardware-interfaces)
   6.3 [Software Interfaces](#63-software-interfaces)
   6.4 [Communications Interfaces](#64-communications-interfaces)

7. [System Constraints](#7-system-constraints)

8. [Legal and Regulatory Requirements](#8-legal-and-regulatory-requirements)

9. [Deployment Requirements](#9-deployment-requirements)

10. [Maintenance and Support](#10-maintenance-and-support)

11. [Glossary](#11-glossary)

12. [Appendices](#12-appendices)



## 1. Introduction
### 1.1 Purpose

The purpose of this document is to outline the software requirements for the BugNet Core web application. This document will provide a detailed description of the system’s functionalities, interfaces, and constraints to guide the development process.

### 1.2 Scope

The BugNet Core is a robust, web-based application designed to streamline the process of managing and tracking software bugs across various projects. Key features include:

- User-friendly bug reporting interface for customers
- Efficient bug management system for developers and administrators
- Real-time chat functionality for urgent customer support
- Comprehensive notification system to keep all parties informed
- Project management capabilities for organizing bug-related activities

The system aims to enhance communication between customers, developers, and support staff, ultimately improving the efficiency of the bug resolution process and overall software quality.

### 1.3 Definitions and Acronyms

- Customer: An individual or entity that has purchased a copy of the software/project
- Dev: A software developer employed by the company
- Admin: A software developer with elevated administrative privileges
- User: A general term encompassing all types of system users (Customers, Devs, and Admins)
- Project Association: A user is considered part of a project if they are an Admin, a Developer assigned to the project, or a Customer of the project
- Bug Association: A user is associated with a bug if they are an Admin, a Developer assigned to fix the bug, or a Customer who reported the bug
## 2. Functional Requirements

### 2.1. User Management

- Notes:
	- All the functionalities below require a logged user unless specified otherwise.
#### 2.1.1 User Registration

- **Requirement:** The system shall allow new customers to register to the system.
- **Input:** Username, Email, Password.
- **Process:** User submits the registration form.
- **Output:** User account is created, and a confirmation email is sent to the provided email address.
- **Validation:**  email must be unique. Password must meet complexity requirements (must be more than 8 chars)

#### 2.1.2 User Login

- **Requirement:** The system shall allow users to log in using their email and password.
- **Input:** Username, Password.
- **Process:** User submits the login form.
- **Output:** User is authenticated and redirected to their dashboard.
- **Validation:** Username and password must match an existing user account.

#### 2.1.3 Reset Password

- **Requirement:** The system shall allow users reset their passwords.
- **Input:** Valid user email, New password
- **Process:** User navigates the reset password page, submits a reset password form entering his email then a URI to reset his password is sent to his email. The user should log to his email and click the reset password URI sent to him, then the browser will route him to page where he can write the new password he wants and confirm it.
- **Output:** Password changed.
- **Validation:** Email should belong to an existing user. Password must meet complexity requirements (must be more than 8 chars)

#### 2.1.4 User Profile Management

- **Requirement:** The system shall allow users to view and update their profile information.
- **Input:** Updated profile details (e.g., email, password).
- **Process:** User submits the updated profile form.
- **Output:** Profile information is updated in the system.
- **Validation:** New email must be unique if changed. Password must meet complexity requirements if changed.

#### 2.1.5 View Users
- **Requirement:** The system shall allow Admins to view all users, and optionally filter or sort them based on a property.
- **Input:** 
- **Process:** Admin navigate to the users page
- **Output:** User info displayed
- **Validation:** The logged user must have then Admin role.


### 2.2. Project Management

#### 2.2.1 Create Project

- **Requirement:** The system shall allow Admins to create a new project.
- **Input:** Project Name, Project Description.
- **Process:** Admin submits the project creation form.
- **Output:** Project is added to the system, and a confirmation message is displayed.
- **Validation:** Project name must be unique, logged user must have the Admin role.

#### 2.2.2 View Projects

- **Requirement:** The system shall allow users to view a list of all projects they are part of, and optionally filter or sort them based on a property.
- **Input:** 
- **Process:** User navigates to the projects page.
- **Output:** List of projects is displayed with project name, description and Status.


#### 2.2.3 Update Project

- **Requirement:** The system shall allow Admins to Edit all the details related to a project..
- **Input:** Project ID
- **Process:** Admin navigates to the projects page, filters projects enter the editing page.
- **Output:** Project details updated.
- **Validation:** The logged user must have the Admin role


### 2.3. Bug Management

#### 2.3.1 Report Bug

- **Requirement:** The system shall allow customers to report a bug by providing details.
- **Input:** Project Name, Category, Description, UserAssignedPriority, Screenshot.
- **Process:** User submits the bug report form.
- **Output:** Bug is added to the system, and a confirmation message is displayed.
- **Validation:** All fields must be filled. Screenshot file must be a valid image format.

#### 2.3.2 View Bugs

- **Requirement:** The system shall allow users to view the list of bugs associated with a project (customers see only the bugs associated with them), and optionally filter or sort them based on a property.
- **Input:**  
- **Process:** User navigates to the bugs page.
- **Output:** List of bugs is displayed with details such as bug ID, title, description, status, priority, and assignee.
#### 2.3.3 Update Bug

- **Requirement:** The system shall allow Admins and the Dev assigned to update the details of a specific bug.
- **Input:** filed that needs updating
- **Process:** User submits the update bug form.
- **Output:** Bug details are updated in the system, and a confirmation message is displayed.
- **Validation:**  Updated details must be valid. User must have the Dev role or the Admin role to update the Bug Status. User must have the Admin role to able to edit all fields. 

*Notes:
1. ==Admin== can assign  a Dev when updating the Bug.
### 2.4. Comment Management


#### 2.4.1 Add/Create Comment

- **Requirement:** The system shall allow Admins and Devs to add a comment on a specific bug (that is associated to them).
- **Input:** Bug ID, Comment Text.
- **Process:** User submits the add comment form.
- **Output:** Comment is added to the bug, and a confirmation message is displayed.
- **Validation:**  BugID must exist, User ID must be associated with the Bug, User must have the Admin role or the Dev role.

#### 2.4.2 View Comments
- **Requirement:** The system shall allow Admins and Devs to view comments associated with a specific bug (that is associated with them).
- **Input:** Bug ID.
- **Process:** User navigates to the bug details page.
- **Output:** List of comments is displayed with comment text and author.
- **Validation:** Bug ID must exist, User ID must exist, User ID must be associated with the Bug, User must either have the Admin or the Developer role.

### 2.5. Notification Management

#### 2.5.1 Receive Notifications

- **Requirement:** The system shall send notifications to users based on specific events (bug updates and new comments | chat rooms invitations | Live Support request)
- **Input:** 
- **Process:** User sign into the system, then the system send updated Notifications about Bug updates and Comments.
- **Output:** User receives the notification.
- **Validation:** User ID must exist .

#### 2.5.2 Get Unread Notifications

- **Requirement:** The system shall send all unread Notifications to the relevant users once sign into the system.
- **Input:** 
- **Process:** User sign into the system, the system send display all unread Notification.
- **Output:** List of unread notifications is displayed.
- **Validation:** User ID must exist.

### 2.6. Live Support Management
#### 2.6.3. Request Live Support

- **Requirement:** The system shall allow customers to request live support for critical issues (This notifies all online admins with the .
- **Input:** Bug ID.
- **Process:** Customer navigates to the Home page, submits a Bug report, then navigates to the Bug details page and submits a Live Support Request.
- **Output:** Notification is sent to all Admins.
- **Validation:** Bug ID must exist. Logged user must have the customer role. The logged Customer must be the one reported the Bug. An approved request must not already exist on that Bug.

#### 2.6.4. Cancel Live Support Request

- **Requirement:** The system shall allow customers to cancel a pending live support request they made.
- **Input:** Request ID.
- **Process:** Customer navigates to Requests page, selects a pending request then press the cancel button.
- **Output:** Notification is sent to all Admins.
- **Validation:** Request ID must exist. Request must be in the pending status. The logged user must have the customer role.

#### 2.6.5. Reject a Live Support Request

- **Requirement:** The system shall allow Admins to reject a live support request made by a customer.
- **Input:** Request ID.
- **Process:** Admin navigates to the requests page, selects an active request, then reject the request.
- **Output:** Notification is sent to the customer.
- **Validation:** Request ID must exist, Request must be in the pending status. Logged user must have the Admin role.

#### 2.6.6. Approve a Live Support Request

- **Requirement:** The system shall allow Admins to approve a live support request made by a customer, and assign a live support agent (a Dev) to help the customer.
- **Input:** Request ID, User ID (for the Dev to be assigned)
- **Process:** Admin navigates to the requests page, selects an active request, then selects a developer and approves the request.
- **Output:** Notification is sent to the customer and the assigned developer, with invitation (a URI) to a Chatting Room.
- **Validation:** Request ID must exist, Request must be in the Pending or Rejected status. Assigned user must have the Dev role. Logged user must have the Admin role.

#### 2.6.7. View Requests

- **Requirement:** The system shall allow users to view all requests (a customer see only the requests they made, a Dev see the request he is assigned to, an Admin see all requests) , and optionally filter or sort them based on a property.
- **Input:** 
- **Process:** User navigates to the requests page.
- **Output:** Live support request are displayed.
- **Validation:**

#### 2.6.8. View Request Details

- **Requirement:** The system shall allow users to view details for all requests (a customer see only the requests they made, a Dev see the request he is assigned to, an Admin see all requests).
- **Input:** Request ID
- **Process:** User navigates to the requests page. Choose a request to view it's details.
- **Output:** Live support request details are displayed.
- **Validation:** 


### 2.7. Chat Room Management

#### 2.7.1. Join Chat Room

- **Requirement:** The system shall allow users invited into a Chat Room to join and exchange messages.
- **Input:** Request ID.
- **Process:** User clicks the invitation links from the Notifications.
- **Output:** User is redirected to the Chat Room page, User is able to send and receive messages and leave the Room when wanted.
- **Validation:** Logged user must be part of the Chat Room

#### 2.7.2. View Chat Room messages

- **Requirement:** The system shall allow users to view messages in any Chat Room they have been part of.
- **Input:** Request ID.
- **Process:** User navigates to the requests page, then the request details page then click the see Chat Room button.
- **Output:** Messages exchanged are displayed.
- **Validation:** Logged user must be part of the Chat Room or must have the Admin role.


## 3. Non-Functional Requirements

### 3.1 Performance Requirements
- The system shall support at least 1000 concurrent users without degradation in performance.
- All API requests should have a response time of less than 500ms under normal load conditions.
- The real-time chat and notification system should deliver messages with a latency of less than 2 seconds.

### 3.2 Security Requirements
- All communication between the client and server must be encrypted using HTTPS.
- User passwords must be hashed and salted before storing in the database.
- The system must implement proper authentication and authorization mechanisms for all API endpoints.
- The system should be resistant to common web vulnerabilities (e.g., SQL injection, XSS, CSRF).

### 3.3 Scalability Requirements
- The system architecture should allow for horizontal scaling to handle increased load.

### 3.4 Reliability Requirements
- The system should have an uptime of at least 99.9%.

### 3.5 Usability Requirements
- The user interface should be responsive and compatible with major web browsers (Chrome, Firefox, Safari, Edge).
- The system should provide clear error messages and user-friendly notifications.

### 3.6 Compatibility Requirements
- The API should follow RESTful principles and use JSON for data exchange.
- The system should provide WebSocket support for real-time features.

## 4. System Architecture


### 4.1 High-Level Architecture

The BugNet Core system follows a modern, scalable architecture using ASP.NET Core for the backend API and React for the frontend. Here's an overview of the key components:

1. **Frontend (React)**
   - Single Page Application (SPA)
   - Uses React Router for navigation
   - State management with Redux or Context API
   - Responsive design using CSS frameworks (e.g., MaterialUI, Tailwind CSS)

2. **Backend (ASP.NET Core)**
   - RESTful API built with ASP.NET Core
   - Uses Entity Framework Core for data access

3. **Authentication & Authorization**
   - JSON Web Tokens (JWT) for authentication
   - Role-based access control (RBAC) for authorization

4. **Real-time Communication**
   - SignalR for WebSocket connections
   - Used for live chat and real-time notifications

5. **Database**
   - SQL Server for relational data storage

6. **API Gateway**
   - Implements rate limiting and request throttling
   - Handles cross-cutting concerns like logging and monitoring

7. **Containerization & Orchestration**
   - Docker for containerization

8. **Monitoring & Logging**
   - Application Insights for performance monitoring and analytics
   - Serilog for structured logging

9. **CI/CD**
   - Automated testing at multiple levels (unit, integration, e2e)

### 4.2 Database Design

The BugNet Core system uses a relational database (SQL Server) with the following schema:

#### Tables

##### User
- UserID (PK)
- Role (ENUM: Admin, Customer, Dev)
- Email (UNIQUE)
- Username
- Password
- Picture (VARCHAR - stores file path)
- Bio
- CreatedAt
- LastModified
- TimeStamp (for concurrency control)

##### Project
- ProjectID (PK)
- Name (UNIQUE)
- Description
- Status (ENUM: Active, Deprecated, Terminated)
- CreatedAt
- LastModified
- TimeStamp (for concurrency control)

##### Bug
- BugID (PK)
- ProjectID (FK)
- CustomerID (FK)
- DevID (FK)
- Category (ENUM: UI, Database, Backend)
- Description
- UserAssignedPriority (ENUM: Urgent, High, Medium, Low)
- AdminAssignedPriority (ENUM: High, Medium, Low)
- Status (ENUM: Reported, InProgress, Testing, Resolved)
- Screenshot (VARCHAR - stores file path)
- CreatedAt
- LastModified
- TimeStamp (for concurrency control)

##### Comment
- CommentID (PK)
- SenderID (FK)
- BugID (FK)
- CommentText
- SentAt
- LastModified
- TimeStamp (for concurrency control)

##### Notification
- NotificationID (PK)
- Type (ENUM: Bug, Comment, ChatInvitation)
- Message
- IsRead
- CreatedAt
- LastModified
- TimeStamp (for concurrency control)

##### SupportRequest
- RequestID (PK)
- BugID (FK)
- Status (ENUM: Pending, Approved, Canceled, Rejected, Closed)
- CreatedAt
- LastModified
- TimeStamp (for concurrency control)

##### ChatRoom
- RoomID (PK)
- RequestID (FK)
- SupportDevID (FK)
- CreatedAt
- LastModified
- TimeStamp (for concurrency control)

##### ChatMessage
- MessageID (PK)
- ChatRoomID (FK)
- SenderID (FK)
- MessageText
- SentAt
- TimeStamp (for concurrency control)

#### Relationships

##### Many-to-Many Relationships
- UserProject (join table)
  - UserID (FK)
  - ProjectID (FK)
  - Role (ENUM: Owner, Member)

- UserNotification (join table)
  - UserID (FK)
  - NotificationID (FK)


![ER Diagram for the Database Design](<ERD Diagram.png>)

#### Indexes
- Create appropriate indexes on frequently queried columns (e.g., foreign keys, email, creation dates)

#### Constraints
- Implement foreign key constraints to maintain referential integrity
- Use CHECK constraints for enum-like columns



### 4.3 API Design
The API is designed following RESTful principles and includes endpoints for:

- Authentication (Register, Login, Change Password)
- User Management (CRUD operations)
- Project Management (CRUD operations)
- Bug Management (CRUD operations)
- Comment Management (CRUD operations)
- Notification Management (WebSocket and HTTP endpoints)
- Live Support Management (Create, Read, Update operations)
- Chat Room Management (WebSocket and HTTP endpoints)

[Click here](./API%20Design%20Document.md) for the detailed API Design Documention

## 5. Data Requirements

### 5.1 Data Entities
The main data entities in the system are:

1. User
2. Project
3. Bug
4. Comment
5. Notification
6. Support Request
7. Chat Room
8. Chat Message

### 5.2 Data Retention
- Bug data should be retained for at least 5 years.
- Chat logs should be retained for 1 year.
- User account data should be retained as long as the account is active, and for 30 days after account deletion.

### 5.3 Data Backup
- Full database backups should be performed daily.
- Incremental backups should be performed hourly.
- Backups should be stored in a geographically separate location from the primary database.

## 6. External Interfaces

### 6.1 User Interfaces
- The system will provide a web-based user interface accessible via modern web browsers.
- The UI will be responsive and support both desktop and mobile devices.

### 6.2 Hardware Interfaces
- No specific hardware interfaces are required for this web-based system.

### 6.3 Software Interfaces
- The system will interface with an SMTP server for sending email notifications.
- Integration with version control systems (e.g., Git) may be considered for future enhancements.

### 6.4 Communications Interfaces
- The system will use HTTPS for secure communication between the client and server.
- WebSocket protocol will be used for real-time features (chat and notifications).

## 7. System Constraints

- The system must be developed using web technologies compatible with modern browsers.
- The backend should be developed using a language and framework that support high concurrency (e.g., C#, Node.js, Go, or Java with Spring Boot).
- The database must be a relational database management system (e.g., PostgreSQL, MySQL).

## 8. Legal and Regulatory Requirements

- The system should implement appropriate measures to protect sensitive user information.
- Terms of Service and Privacy Policy documents must be provided and easily accessible to users.

## 9. Deployment Requirements

- The system should be containerized using Docker for easy deployment and scaling.
- The system should be deployable on major cloud platforms (AWS, Google Cloud, or Azure).

## 10. Maintenance and Support

- The system should log all errors and exceptions for easy troubleshooting.
- Administrative tools should be provided for user management, system monitoring, and basic database operations.
- Documentation should be provided for system administration, including deployment instructions and troubleshooting guides.

## 11. Glossary

- Bug: An error, flaw, or fault in the software that causes it to produce an incorrect or unexpected result, or to behave in unintended ways.
- User: Any individual interacting with the system (Customer, Developer, or Admin).
- Project: A software project that is being tracked for bugs.
- Live Support: Real-time assistance provided to customers for critical issues.

## 12. Appendices

- Appendix A: [Database Schema Diagram](ERD%20Diagram.png)
- Appendix B: [API Documentation](./API%20Design%20Document.md)
