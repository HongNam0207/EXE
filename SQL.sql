Create database AILENS
use AILENS
-- 1. Bảng users
CREATE TABLE users (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    name NVARCHAR(100) NOT NULL,
    email NVARCHAR(255) NOT NULL UNIQUE,
    password_hash NVARCHAR(MAX),
    role NVARCHAR(50) CHECK (role IN ('student', 'guest', 'admin')),
    avatar_url NVARCHAR(500),
    login_method NVARCHAR(50) DEFAULT 'email',
    created_at DATETIME DEFAULT GETDATE()
);

-- 2. Bảng auth_providers
CREATE TABLE auth_providers (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    user_id UNIQUEIDENTIFIER FOREIGN KEY REFERENCES users(id),
    provider NVARCHAR(50),
    provider_uid NVARCHAR(255),
    access_token NVARCHAR(MAX),
    refresh_token NVARCHAR(MAX),
    created_at DATETIME DEFAULT GETDATE()
);

-- 3. Bảng locations
CREATE TABLE locations (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    name NVARCHAR(255) NOT NULL,
    type NVARCHAR(100) CHECK (type IN ('history', 'nature', 'building', 'service', 'other')),
    description NVARCHAR(MAX),
    coordinates NVARCHAR(MAX),
    image_url NVARCHAR(500),
    qr_code_url NVARCHAR(500),
    glb_model_url NVARCHAR(500),
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE()
);

-- 4. Bảng topics
CREATE TABLE topics (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    name NVARCHAR(100) NOT NULL,
    description NVARCHAR(MAX)
);

-- 5. Bảng location_topics
CREATE TABLE location_topics (
    location_id UNIQUEIDENTIFIER FOREIGN KEY REFERENCES locations(id),
    topic_id UNIQUEIDENTIFIER FOREIGN KEY REFERENCES topics(id),
    PRIMARY KEY (location_id, topic_id)
);

-- 6. Bảng chat_logs
CREATE TABLE chat_logs (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    user_id UNIQUEIDENTIFIER FOREIGN KEY REFERENCES users(id),
    question NVARCHAR(MAX),
    response NVARCHAR(MAX),
    timestamp DATETIME DEFAULT GETDATE()
);

-- 7. Bảng maps
CREATE TABLE maps (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    location_id UNIQUEIDENTIFIER FOREIGN KEY REFERENCES locations(id),
    map_data NVARCHAR(MAX)
);

-- 8. Bảng tours
CREATE TABLE tours (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    title NVARCHAR(255) NOT NULL,
    type NVARCHAR(50) CHECK (type IN ('virtual', 'guide', 'custom')),
    location_ids NVARCHAR(MAX)
);

-- 9. Bảng admin_logs
CREATE TABLE admin_logs (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    admin_id UNIQUEIDENTIFIER FOREIGN KEY REFERENCES users(id),
    action NVARCHAR(MAX),
    timestamp DATETIME DEFAULT GETDATE()
);

-- 10. Bảng analytics
CREATE TABLE analytics (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    user_id UNIQUEIDENTIFIER FOREIGN KEY REFERENCES users(id),
    location_id UNIQUEIDENTIFIER FOREIGN KEY REFERENCES locations(id),
    action NVARCHAR(50) CHECK (action IN ('view', 'navigate', 'search')),
    timestamp DATETIME DEFAULT GETDATE()
);
