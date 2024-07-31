
# API Design Document

## Table of Contents

1. [Auth API](#auth-api)
    - [Register](#register)
    - [Login](#login)
    - [Request Password Reset](#request-password-reset)
    - [Confirm Reset Password](#confirm-reset-password)
2. [Users API](#users-api)
    - [Create a User](#create-a-user)
    - [Get a User](#get-a-user)
    - [Get all Users](#get-all-users)
    - [Update a User](#update-a-user)
    - [Delete a User](#delete-a-user)
3. [Projects API](#projects-api)
    - [Create a Project](#create-a-project)
    - [Get all Projects](#get-all-projects)
    - [Get a Project](#get-a-project)
    - [Update a Project](#update-a-project)
    - [Delete a Project](#delete-a-project)
4. [Bugs API](#bugs-api)
    - [Create a Bug / Report a Bug](#create-a-bug--report-a-bug)
    - [Get all Bugs](#get-all-bugs)
    - [Get a Bug](#get-a-bug)
    - [Update a Bug](#update-a-bug)
    - [Delete a Bug](#delete-a-bug)
5. [Comments API](#comments-api)
    - [Add/Create a Comment](#addcreate-a-comment)
    - [Get all Comments](#get-all-comments)
    - [Update a Comment](#update-a-comment)
    - [Delete a Comment](#delete-a-comment)
6. [Notifications API](#notifications-api)
    - [WebSocket Endpoints](#websocket-endpoints)
        - [Subscribe to Notifications](#subscribe-to-notifications)
    - [HTTP Endpoints](#http-endpoints)
        - [Get All Unread Notifications](#get-all-unread-notifications)
7. [Chat Room API](#chat-room-api)
    - [WebSocket Endpoints](#websocket-endpoints-1)
        - [Subscribe to Chat](#subscribe-to-chat)
        - [Join Chat Room](#join-chat-room)
        - [Receive Message](#receive-message)
        - [Send Message](#send-message)
        - [Leave Room](#leave-room)
    - [HTTP Endpoints](#http-endpoints-1)
        - [Get Chat Room Messages](#get-chat-room-messages)
8. [Live Support API](#live-support-api)
    - [Create a Support Request](#create-a-support-request)
    - [Get all Support Requests](#get-all-support-requests)
    - [Get a Support Request](#get-a-support-request)
    - [Update a Support Request](#update-a-support-request)

9. [Error Handling](#error-handling)
    - General Approach
    - HTTP Status Codes
    - Error Response Format
    - Validation Errors
## Auth API

### Register

#### Register Request

```js
POST api/v1-beta/auth/register
```

```json
{
    "name": "John Doe",
    "email": "johndoe@example.com",
    "password": "password123"
}
```

#### Register Response

```js
202 Accepted
```

```json
{
    "message": "Verification email has been sent. Please check your inbox."
}
```

### Login

#### Login Request

```js
POST api/v1-beta/auth/login
```

```json
{
    "email": "johndoe@example.com",
    "password": "password123"
}
```

#### Login Response

```js
200 Ok
```

```json
{
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "message": "Login successful"
}
```

###  Request Password Reset

#### Request Password Reset Request 

```js
PUT api/v1-beta/auth/reset-password/request
```

```json
{
    "email": "johndoe@example.com",
}
```

#### Request Password Reset Response

```js
200 Ok
```

```json
{
    "message": "A password reset link has been sent to your email address. Please check your inbox and follow the instructions to reset your password."
}
```


### Confirm Reset Password 

#### Confirm Reset Password Request

```js
PUT api/v1-beta/auth/reset-password/confirm/{token}
```

```json
{
    "newPassword": "newpassword123"
}
```

#### Confirm Reset Password Response

```js
200 Ok
```

```json
{
    "message": "Your password has been successfully reset. You can now log in with your new password."
}
```





## Users API

### Create a User

#### Create a User Request

```js
POST api/v1-beta/user
```

```json
{
    "Role": "Admin",
    "Email": "admin@example.com",
    "Username": "admin",
    "Password": "password",
    "Picture": "profile.jpg",
    "Bio": "Administrator",
}
```

#### Create a User Response

```js
201 Created
```

### Get a User

#### Get a User Request

```js
GET api/v1-beta/user/{{id}}
```

#### Get a User Response

```js
200 OK
```

```json
{
    "UserID": 1,
    "Role": "Admin",
    "Email": "admin@example.com",
    "Username": "Admin",
    "Password": "password",
    "Picture": "profile.jpg",
    "Bio": "Administrator",
    "CreatedAt": "2024-07-25T00:00:00Z",
	"TimeStamp": "AAAAAAAAB9s="
}
```


### Get all Users

#### Get all Users Request

```js
GET api/v1-beta/user?
// filterOn=id&
// filterQuery=2&
sortBy=id&
isAscending=true&
pageSize=10&
pageNumber=1
```

#### Get all Users Response

```js
200 OK
```

```json
[
    {
	    "UserID": 1,
	    "Role": "Admin",
	    "Email": "admin@example.com",
	    "Username": "Admin",
	    "Password": "password",
	    "Picture": "profile.jpg",
	    "Bio": "Administrator",
	    "CreatedAt": "2024-07-25T00:00:00Z",
		"TimeStamp": "AAAAAAAAB9s="
	},
    {
	    "UserID": 2,
	    "Role": "Dev",
	    "Email": "dev@example.com",
	    "Username": "dev",
	    "Password": "password",
	    "Picture": "profile.jpg",
	    "Bio": "Backend developer 1",
	    "CreatedAt": "2024-07-25T00:00:00Z",
		"TimeStamp": "AAAAAAAAB9s="
	}
]
```

### Update a User

#### Update a User Request

```js
PUT api/v1-beta/user/{{id}}
```

```json
{
    "Id": 2,
    "Role": "Customer",
    "Email": "customer@example.com",
    "Username": "customer",
    "Picture": "newprofile.jpg",
    "Bio": "Updated bio",
	"TimeStamp": "AAAAAAAAB9s="
}
```

#### Update a User Response

```js
200 OK
```

### Delete a User

#### Delete a User Request

```js
DELETE api/v1-beta/user/{{id}}
```

```json
{
    "Id": 2,
	"TimeStamp": "AAAAAAAAB9s="
}
```
#### Delete a User Response

```js
204 No Content
```


## Projects API

### Create a Project

#### Create a Project Request

```js
POST api/v1-beta/project
```

```json
{
    "Name": "Project Alpha",
    "Description": "This is a project description",
}
```

#### Create a Project Response

```js
201 Created
```


### Get all Projects

#### Get all Projects Request

```js
GET api/v1-beta/project?
// filterOn=id&
// filterQuery=2&
sortBy=id&
isAscending=true&
pageSize=10&
pageNumber=1
```

#### Get all Projects Response

```js
200 OK
```

```json
[
	{
	    "ProjectID": 1,
	    "Name": "Project Alpha",
	    "Description": "This is a project description",
	    "Status": "Active",
	    "CreatedAt": "2024-07-25T00:00:00Z",
		"TimeStamp": "AAAAAAAAB9s="
	},
   {
	    "ProjectID": 2,
	    "Name": "Project Beta",
	    "Description": "This is a project description",
	    "Status": "Terminated",
	    "CreatedAt": "2024-07-25T00:00:00Z",
		"TimeStamp": "AAAAAAAAB9s="
	},
]
```

### Get a Project

#### Get a Project Request

```js
GET api/v1-beta/project/{{id}}
```

#### Get a Project Response

```js
200 OK
```

```json
{
    "ProjectID": 1,
    "Name": "Project Alpha",
    "Description": "This is a project description",
    "Status": "Active",
    "CreatedAt": "2024-07-25T00:00:00Z",
	"TimeStamp": "AAAAAAAAB9s="
}
```

### Update a Project

#### Update a Project Request

```js
PUT api/v1-beta/project/{{id}}
```

```json
{
	"Id": 2,
    "Name": "Project Beta",
    "Description": "This is an updated project description",
    "Status": "Deprecated",
	"TimeStamp": "AAAAAAAAB9s="
}
```

#### Update a Project Response

```js
200 OK
```

### Delete a Project

#### Delete a Project Request

```js
DELETE api/v1-beta/project/{{id}}
```

```json
{
	"Id": 2,
	"TimeStamp": "AAAAAAAAB9s="
}
```
#### Delete a Project Response

```js
204 No Content
```

## Bugs API

### Create a Bug / Report a Bug

#### Create a Bug Request

```js
POST api/v1-beta/bug
```

```json
{
    "ProjectID": 1,
    "CustomerID": 2,
    "Category": "UI",
    "Description": "Button not working",
    "UserAssignedPriority": "High",
    "ScreenShot": "screenshot.jpg",
}
```

#### Create a Bug Response

```js
201 Created
```


### Get all Bugs

#### Get all Bugs Request

```js
GET api/v1-beta/bug?
// filterOn=id&
// filterQuery=2&
sortBy=id&
isAscending=true&
pageSize=10&
pageNumber=1
```

#### Get all Bugs Response

```js
200 OK
```

```json
[
	{
	    "BugID": 1,
	    "ProjectID": 1,
	    "CustomerID": 2,
	    "DevID": 3,
	    "Category": "UI",
	    "Description": "Button not working",
	    "UserAssignedPriority": "High",
	    "AdminAssignedPriority": "Medium",
	    "Status": "Open",
	    "ScreenShot": "screenshot.jpg",
	    "CreatedAt": "2024-07-25T00:00:00Z",
		"TimeStamp": "AAAAAAAAB9s="
	},
	{
	    "BugID": 2,
	    "ProjectID": 1,
	    "CustomerID": 2,
	    "DevID": 3,
	    "Category": "UI",
	    "Description": "Button not working",
	    "UserAssignedPriority": "High",
	    "AdminAssignedPriority": "Medium",
	    "Status": "InProgress",
	    "ScreenShot": "screenshot.jpg",
	    "CreatedAt": "2024-07-25T00:00:00Z",
		"TimeStamp": "AAAAAAAAB9s="
	},
]
```

### Get a Bug

#### Get a Bug Request

```js
GET /bug/{{id}}
```

#### Get a Bug Response

```js
200 OK
```

```json
{
    "BugID": 1,
    "ProjectID": 1,
    "CustomerID": 2,
    "DevID": 3,
    "Category": "UI",
    "Description": "Button not working",
    "UserAssignedPriority": "High",
    "AdminAssignedPriority": "Medium",
    "Status": "Open",
    "ScreenShot": "screenshot.jpg",
    "CreatedAt": "2024-07-25T00:00:00Z",
	"TimeStamp": "AAAAAAAAB9s="
}
```

### Update a Bug

#### Update a Bug Request

```js
PUT api/v1-beta/bug/{{id}}
```

```json
{
    "BugID": 1,
    "Category": "Backend",
    "Description": "API not responding",
    "UserAssignedPriority": "Low",
    "AdminAssignedPriority": "High",
    "Status": "In Progress",
    "ScreenShot": "updated_screenshot.jpg",
	"TimeStamp": "AAAAAAAAB9s="
}
```

#### Update a Bug Response

```js
200 OK
```

### Delete a Bug

#### Delete a Bug Request

```js
DELETE api/v1-beta/bug/{{id}}
```

```json
{
    "BugID": 1,
	"TimeStamp": "AAAAAAAAB9s="
}
```
#### Delete a Bug Response

```js
204 No Content
```


## Comments API

### Add/Create a Comment

#### Create a Comment Request

```js
POST api/v1-beta/comment
```

```json
{
    "SenderID": 1,
    "BugID": 1,
    "CommentText": "This is a comment",
}
```

#### Create a Comment Response

```js
201 Created
```


### Get all Comments

#### Get all Comments by Bug filtering Request

```js
GET api/v1-beta/comment??
filterOn=BugId&
filterQuery=2&
sortBy=SentAt&
isAscending=true&
pageSize=10&
pageNumber=10
```

#### Get all Comments by Bug Response

```js
200 OK
```

```json
[
    {
        "CommentID": 1,
        "SenderID": 1,
        "BugID": 1,
        "CommentText": "This is a comment",
        "SentAt": "2024-07-25T00:00:00Z",
		"TimeStamp": "AAAAAAAAB9s="
    },
    {
        "CommentID": 2,
        "SenderID": 2,
        "BugID": 1,
        "CommentText": "This is another comment",
        "SentAt": "2024-07-26T00:00:00Z",
		"TimeStamp": "AAAAAAAAB9s="
    }
]
```


### Update a Comment

#### Update a Comment Request

```js
PUT api/v1-beta/comment/{{id}}
```

```json
{
	"CommentID": 2,
    "CommentText": "This is an updated comment",
	"TimeStamp": "AAAAAAAAB9s="
}
```

#### Update a Comment Response

```js
200 OK
```

### Delete a Comment

#### Delete a Comment Request

```js
DELETE api/v1-beta/comment/{{id}}
```

```json
{
	"CommentID": 2,
	"TimeStamp": "AAAAAAAAB9s="
}
```
#### Delete a Comment Response

```js
204 No Content
```

## Notifications API

### WebSocket Endpoints

#### Subscribe to Notifications

```js
ws://{domain}/ws/notifications
```

##### Client Hook to listen to incoming notifications `ReceiveNotifications`

```json
{
    "NotificationID": 1,
    "Type": "Bug",
    "Message": "New bug assigned to you",
    "IsRead": false,
    "CreatedAt": "2024-07-25T15:00:00Z",
	"AdditionalInfo": // Info to navigate to Bug details page
		{
			"BugID": 2
		}
}
```

### HTTP Endpoints

#### Get All Unread Notifications

##### Get All Unread Notifications Request

```js
GET /notifications/unread
```

##### Get All Unread Notifications Response

```json
[
    {
        "NotificationID": 1,
        "Type": "Bug",
        "Message": "New bug assigned to you",
        "IsRead": false,
        "CreatedAt": "2024-07-25T15:00:00Z",
        "AdditionalInfo": // Info to navigate to Bug details page
	        {
		        "BugID": 2
	        }
    },
    {
        "NotificationID": 2,
        "Type": "Comment",
        "Message": "New comment on your bug",
        "IsRead": false,
        "CreatedAt": "2024-07-24T12:00:00Z",
        "AdditionalInfo": // Info to navigate to Bug details page
	        {
		        "BugID": 5
	        }
        
    },
    {
        "NotificationID": 2,
        "Type": "SupportRequest",
        "Message": "Customer requesting urgent live support !",
        "IsRead": false,
        "CreatedAt": "2024-07-24T12:00:00Z",
        "AdditionalInfo": // Info to navigate a request to accept/reject
	        {
		        "RequestID": 3
	        }
        
    },
    {
        "NotificationID": 3,
        "Type": "ChatInvitation",
        "Message": "Support request approved, click to join Chat Room",
        "IsRead": false,
        "CreatedAt": "2024-07-24T12:00:00Z",
        "AdditionalInfo": // Info to create URI to join room
	        {
		        "ReqestID": 20,
	        }
    }
]
```



## Chat Room API

### WebSocket Endpoints

#### Subscribe to Chat 

```js
ws://{domain}/ws/chat
```

##### Invokable Hub Hook to join a chat room `Join`

###### Join Chat Room Request
```json
{
    "RequestID": 1
}
```
###### Join Chat Room Response - Existing messages are sent back to the user once he  joins the Room -

```json
{
    "Messages": [
        {
            "MessageID": 1,
            "SenderID": 1,
            "MessageText": "Hello, how can I help you?",
            "SentAt": "2024-07-25T15:00:00Z"
        },
        {
            "MessageID": 2,
            "SenderID": 2,
            "MessageText": "I need help with a client payment!",
            "SentAt": "2024-07-25T15:00:00Z"
        }
    ]
}
```

##### Hook to listen to incoming messages in the `ReceiveMessage<RequestID>`
###### Receive Message Response - New Message Sample -

```json
{
    "MessageID": 2,
    "SenderID": 2,
    "MessageText": "I have an issue with my app!",
    "SentAt": "2024-07-25T15:01:00Z"
}
```

##### Hook to send messages `SendMessage`
###### Send Message Request Payload

```json
{
	"RequestID": 2,
    "SenderID": 1,
    "MessageText": "Could you please provide more details?"
}
```

##### Hook to leave room `LeaveRoom`
###### Send Message Request Payload

```json
{
    "RequestID": 1,
}
```




### HTTP Endpoints

#### Get Chat Room Messages - with message filters

##### Get Messages Request

```js
GET api/v1-beta/chat/?
filterOn=MessageText&
filterQuery="press Enter"&
sortBy=SentAt&
// isAscending=true&
pageSize=10&
pageNumber=1
```

##### Get Messages Response

```js
200 OK
```

```json
{
    "Messages": [
        {
            "MessageID": 1,
            "SenderID": 1,
            "MessageText": "*Press Enter* after F2",
            "SentAt": "2024-07-25T15:00:00Z"
        },
        {
            "MessageID": 20,
            "SenderID": 1,
            "MessageText": "Now *press Enter* again",
            "SentAt": "2024-07-25T15:00:00Z"
        }
    ]
}
```


## Live Support API

### Create a Support Request

#### Create a Support Request Request

```js
POST api/v1-beta/support-request
```

```json
{
    "BugID": 1
}
```

#### Create a Support Request Response

```js
201 Created
```

```json
{
    "SupportRequestID": 1,
    "BugID": 1,
    "Status": "Pending",
    "CreatedAt": "2024-07-25T00:00:00Z",
	"TimeStamp": "AAAAAAAAB9s="
}
```

### Get all Support Requests

#### Get all Support Request Request

```js
GET api/v1-beta/support-request?
// filterOn=id&
// filterQuery=2&
sortBy=id&
isAscending=true&
pageSize=10&
pageNumber=1
```

#### Get all Support Request Response

```js
200 OK
```

```json
[
	{
	    "SupportRequestID": 1,
	    "BugID": 1,
	    "Status": "Approved", // Approved, Canceled, Rejected or Closed
	    "CreatedAt": "2024-07-25T00:00:00Z",
		"TimeStamp": "AAAAAAAAB9s="
	},
	{
	    "SupportRequestID": 3,
	    "BugID": 2,
	    "Status": "Canceled", // Approved, Canceled, Rejected or Closed
	    "CreatedAt": "2024-07-25T00:00:00Z",
		"TimeStamp": "AAAAAAAAB9s="
	}
]
```

### Get a Support Request

#### Get a Support Request Request

```js
GET /support-request/{{id}}
```

#### Get a Support Request Response

```js
200 OK
```

```json
{
    "SupportRequestID": 1,
    "BugID": 1,
    "Status": "Pending", // Approved, Canceled, Rejected or Closed
    "CreatedAt": "2024-07-25T00:00:00Z",
	"TimeStamp": "AAAAAAAAB9s="
}
```

### Update a Support Request (based on User's action)

#### Update a Support Request Request 

```js
PUT api/v1-beta/support-request/{{id}}
```

```json
{
    "Action": "approve", // cancel, reject, close
    "RequestID": 5,
    "SupportDevID": 20, // required only if action is "approve"
	"TimeStamp": "AAAAAAAAB9s="
}
```

#### Update a Support Request Response

```js
200 OK
```

```json
{
    "SupportRequestID": 1,
    "BugID": 1,
    "Status": "Approved", // Approved, Canceled, Rejected or Closed
    "CreatedAt": "2024-07-25T00:00:00Z",
	"TimeStamp": "AAAAAAAAB9s="
}
```





## Error Handling

1. **General Approach**
   - All API endpoints should implement consistent error handling.
   - Errors should be logged for debugging and monitoring purposes.
   - Client responses should be informative but not expose sensitive system details.

2. **HTTP Status Codes**
   - 400 Bad Request: Invalid input or request parameters
   - 401 Unauthorized: Authentication failure
   - 403 Forbidden: Authenticated user lacks necessary permissions
   - 404 Not Found: Requested resource doesn't exist
   - 409 Conflict: Request conflicts with current state of the server
   - 422 Unprocessable Entity: Server understands the request but can't process it
   - 500 Internal Server Error: Unexpected server error

3. **Error Response Format**
   ```json
   {
     "error": {
       "code": "ERROR_CODE",
       "message": "A human-readable error message",
       "details": {} // Optional object with additional error details
     }
   }
   ```

4. **Validation Errors**
   - For validation errors, use status code 422 and include details about each invalid field:
   ```json
   {
     "error": {
       "code": "VALIDATION_ERROR",
       "message": "The request contains invalid data",
       "details": {
         "fieldErrors": [
           {
             "field": "email",
             "message": "Invalid email format"
           },
           {
             "field": "password",
             "message": "Password must be at least 8 characters long"
           }
         ]
       }
     }
   }
   ```





