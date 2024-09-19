namespace BugNetCore.Dal.Initialization
{
    public static class SampleData
    {
        public static List<User> Users => new()
        {
            new() { Id = Guid.Parse("a1b2c3d4-e5f6-4a5b-9c8d-1e2f3a4b5c6d"), Username = "admin_user", Email = "admin@example.com", UserRole = Role.Admin, PasswordHash = "$2b$10$KgkHjoekgvvnDbld2WKuNuuOQJPoyjmjlplL7ZNTn44jBAzvXzQRu", Picture = "admin.jpg", Bio = "System administrator", IsVerified= true },
            new() { Id = Guid.Parse("b2c3d4e5-f6a7-5b6c-0d1e-2f3a4b5c6d7e"), Username = "customer1", Email = "customer1@example.com", UserRole = Role.Customer, PasswordHash = "$2b$10$KgkHjoekgvvnDbld2WKuNuuOQJPoyjmjlplL7ZNTn44jBAzvXzQRu", Picture = "customer1.jpg", Bio = "Regular customer", IsVerified= true },
            new() { Id = Guid.Parse("c3d4e5f6-a7b8-6c7d-1e2f-3a4b5c6d7e8f"), Username = "dev_john", Email = "john@example.com", UserRole = Role.Dev, PasswordHash = "$2b$10$KgkHjoekgvvnDbld2WKuNuuOQJPoyjmjlplL7ZNTn44jBAzvXzQRu", Picture = "john.jpg", Bio = "Senior developer", IsVerified= true },
            new() { Id = Guid.Parse("d4e5f6a7-b8c9-7d8e-2f3a-4b5c6d7e8f9a"), Username = "customer2", Email = "customer2@example.com", UserRole = Role.Customer, PasswordHash = "$2b$10$KgkHjoekgvvnDbld2WKuNuuOQJPoyjmjlplL7ZNTn44jBAzvXzQRu", Picture = "customer2.jpg", Bio = "New customer", IsVerified= true },
            new() { Id = Guid.Parse("e5f6a7b8-c9d0-8e9f-3a4b-5c6d7e8f9a0b"), Username = "dev_jane", Email = "jane@example.com", UserRole = Role.Dev, PasswordHash = "$2b$10$KgkHjoekgvvnDbld2WKuNuuOQJPoyjmjlplL7ZNTn44jBAzvXzQRu", Picture = "jane.jpg", Bio = "Junior developer", IsVerified= true }
        };

        public static List<Project> Projects => new()
        {
            new() { Id = Guid.Parse("f6a7b8c9-d0e1-9f0a-4b5c-6d7e8f9a0b1c"), Name = "Project Alpha", Description = "Our flagship project", Status = ProjectStatus.Active },
            new() { Id = Guid.Parse("a7b8c9d0-e1f2-0a1b-5c6d-7e8f9a0b1c2d"), Name = "Project Beta", Description = "Experimental project", Status = ProjectStatus.Active },
            new() { Id = Guid.Parse("b8c9d0e1-f2a3-1b2c-6d7e-8f9a0b1c2d3e"), Name = "Legacy System", Description = "Old system maintenance", Status = ProjectStatus.Deprecated }
        };

        public static List<UserProject> UserProjects => new()
        {
            new() { Id = Guid.Parse("c9d0e1f2-a3b4-2c3d-7e8f-9a0b1c2d3e4f"), UserId = Users[2].Id, ProjectId = Projects[0].Id, Role = UserProjectRole.Admin },
            new() { Id = Guid.Parse("d0e1f2a3-b4c5-3d4e-8f9a-0b1c2d3e4f5a"), UserId = Users[4].Id, ProjectId = Projects[0].Id, Role = UserProjectRole.Member },
            new() { Id = Guid.Parse("e1f2a3b4-c5d6-4e5f-9a0b-1c2d3e4f5a6b"), UserId = Users[2].Id, ProjectId = Projects[1].Id, Role = UserProjectRole.Member },
            new() { Id = Guid.Parse("f2a3b4c5-d6e7-5f6a-0b1c-2d3e4f5a6b7c"), UserId = Users[4].Id, ProjectId = Projects[1].Id, Role = UserProjectRole.Admin },
            new() { Id = Guid.Parse("a3b4c5d6-e7f8-6a7b-1c2d-3e4f5a6b7c8d"), UserId = Users[2].Id, ProjectId = Projects[2].Id, Role = UserProjectRole.Admin }
        };

        public static List<Bug> Bugs => new()
        {
            new() { Id = Guid.Parse("b4c5d6e7-f8a9-7b8c-2d3e-4f5a6b7c8d9e"), Description = "Login page crash", Category = BugCategory.Frontend, CustomerAssignedSeverity = BugSeverity.High, AdminAssignedPriority = BugPriority.High, Status = BugStatus.InProgress, Screenshot = "login_crash.png", ProjectId = Projects[0].Id, CustomerId = Users[1].Id, DevId = Users[2].Id },
            new() { Id = Guid.Parse("c5d6e7f8-a9b0-8c9d-3e4f-5a6b7c8d9e0f"), Description = "Database connection issue", Category = BugCategory.Backend, CustomerAssignedSeverity = BugSeverity.Urgent, AdminAssignedPriority = BugPriority.High, Status = BugStatus.Reported, Screenshot = "db_error.png", ProjectId = Projects[1].Id, CustomerId = Users[3].Id, DevId = null },
            new() { Id = Guid.Parse("d6e7f8a9-b0c1-9d0e-4f5a-6b7c8d9e0f1a"), Description = "UI misalignment on mobile", Category = BugCategory.UI, CustomerAssignedSeverity = BugSeverity.Low, AdminAssignedPriority = BugPriority.Low, Status = BugStatus.Testing, Screenshot = "mobile_ui.png", ProjectId = Projects[0].Id, CustomerId = Users[1].Id, DevId = Users[4].Id }
        };

        public static List<Comment> Comments => new()
        {
            new() { Id = Guid.Parse("e7f8a9b0-c1d2-0e1f-5a6b-7c8d9e0f1a2b"), SenderId = Users[1].Id, BugId = Bugs[0].Id, CommentText = "This is happening on Chrome browser." },
            new() { Id = Guid.Parse("f8a9b0c1-d2e3-1f2a-6b7c-8d9e0f1a2b3c"), SenderId = Users[2].Id, BugId = Bugs[0].Id, CommentText = "Thanks for the info. I'll look into it." },
            new() { Id = Guid.Parse("a9b0c1d2-e3f4-2a3b-7c8d-9e0f1a2b3c4d"), SenderId = Users[3].Id, BugId = Bugs[1].Id, CommentText = "This is affecting all our transactions!" },
            new() { Id = Guid.Parse("b0c1d2e3-f4a5-3b4c-8d9e-0f1a2b3c4d5e"), SenderId = Users[4].Id, BugId = Bugs[2].Id, CommentText = "Fix has been implemented. Please test." }
        };

        public static List<SupportRequest> SupportRequests => new()
        {
            new() { Id = Guid.Parse("c1d2e3f4-a5b6-4c5d-9e0f-1a2b3c4d5e6f"), BugId = Bugs[0].Id, Status = SupportRequestStatus.Approved },
            new() { Id = Guid.Parse("d2e3f4a5-b6c7-5d6e-0f1a-2b3c4d5e6f7a"), BugId = Bugs[1].Id, Status = SupportRequestStatus.Pending },
            new() { Id = Guid.Parse("e3f4a5b6-c7d8-6e7f-1a2b-3c4d5e6f7a8b"), BugId = Bugs[2].Id, Status = SupportRequestStatus.Closed }
        };

        public static List<ChatRoom> ChatRooms => new()
        {
            new() { Id = Guid.Parse("f4a5b6c7-d8e9-7f8a-2b3c-4d5e6f7a8b9c"), SupportRequestId = SupportRequests[0].Id, SupportDevId = Users[2].Id },
            new() { Id = Guid.Parse("a5b6c7d8-e9f0-8a9b-3c4d-5e6f7a8b9c0d"), SupportRequestId = SupportRequests[2].Id, SupportDevId = Users[4].Id }
        };

        public static List<ChatMessage> ChatMessages => new()
        {
            new() { Id = Guid.Parse("b6c7d8e9-f0a1-9b0c-4d5e-6f7a8b9c0d1e"), ChatRoomId = ChatRooms[0].Id, SenderId = Users[1].Id, MessageText = "When will this be fixed?" },
            new() { Id = Guid.Parse("c7d8e9f0-a1b2-0c1d-5e6f-7a8b9c0d1e2f"), ChatRoomId = ChatRooms[0].Id, SenderId = Users[2].Id, MessageText = "We're working on it. Should be done by tomorrow." },
            new() { Id = Guid.Parse("d8e9f0a1-b2c3-1d2e-6f7a-8b9c0d1e2f3a"), ChatRoomId = ChatRooms[1].Id, SenderId = Users[3].Id, MessageText = "Is the fix ready for testing?" },
            new() { Id = Guid.Parse("e9f0a1b2-c3d4-2e3f-7a8b-9c0d1e2f3a4b"), ChatRoomId = ChatRooms[1].Id, SenderId = Users[4].Id, MessageText = "Yes, please test and let me know the results." }
        };

        public static List<Notification> Notifications => new()
        {
            new() { Id = Guid.Parse("f0a1b2c3-d4e5-3f4a-8b9c-0d1e2f3a4b5c"), Type = NotificationType.BugAssignment, Message = "You have been assigned a new bug.", BugId = Bugs[0].Id },
            new() { Id = Guid.Parse("a1b2c3d4-e5f6-4a5b-9c0d-1e2f3a4b5c6d"), Type = NotificationType.Comment, Message = "New comment on your reported bug.", BugId = Bugs[0].Id },
            new() { Id = Guid.Parse("b2c3d4e5-f6a7-5b6c-0d1e-2f3a4b5c6d7e"), Type = NotificationType.ChatInvitation, Message = "You have been invited to a support chat.", SupportRequestId = SupportRequests[0].Id }
        };

        public static List<UserNotification> UserNotifications => new()
        {
            new() { Id = Guid.Parse("c3d4e5f6-a7b8-6c7d-1e2f-3a4b5c6d7e8f"), UserId = Users[2].Id, NotificationId = Notifications[0].Id, IsRead = false },
            new() { Id = Guid.Parse("d4e5f6a7-b8c9-7d8e-2f3a-4b5c6d7e8f9a"), UserId = Users[1].Id, NotificationId = Notifications[1].Id, IsRead = true },
            new() { Id = Guid.Parse("e5f6a7b8-c9d0-8e9f-3a4b-5c6d7e8f9a0b"), UserId = Users[3].Id, NotificationId = Notifications[2].Id, IsRead = false }
        };
    }
}
