--liquibase formatted sql

--changeset student:001_initial_schema
--comment: Initial schema for access control system

-- Users table
CREATE TABLE IF NOT EXISTS users (
    id SERIAL PRIMARY KEY,
    username VARCHAR(100) NOT NULL UNIQUE,
    email VARCHAR(255) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    first_name VARCHAR(100),
    last_name VARCHAR(100),
    is_active BOOLEAN DEFAULT true,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Roles table
CREATE TABLE IF NOT EXISTS roles (
    id SERIAL PRIMARY KEY,
    name VARCHAR(50) NOT NULL UNIQUE,
    description TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Permissions table
CREATE TABLE IF NOT EXISTS permissions (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE,
    description TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Role permissions (many-to-many)
CREATE TABLE IF NOT EXISTS role_permissions (
    role_id INTEGER NOT NULL REFERENCES roles(id) ON DELETE CASCADE,
    permission_id INTEGER NOT NULL REFERENCES permissions(id) ON DELETE CASCADE,
    PRIMARY KEY (role_id, permission_id)
);

-- User roles (many-to-many)
CREATE TABLE IF NOT EXISTS user_roles (
    user_id INTEGER NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    role_id INTEGER NOT NULL REFERENCES roles(id) ON DELETE CASCADE,
    PRIMARY KEY (user_id, role_id)
);

-- Access points table
CREATE TABLE IF NOT EXISTS access_points (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    location VARCHAR(255),
    description TEXT,
    is_active BOOLEAN DEFAULT true,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Access cards table
CREATE TABLE IF NOT EXISTS access_cards (
    id SERIAL PRIMARY KEY,
    card_number VARCHAR(50) NOT NULL UNIQUE,
    card_type VARCHAR(50),
    is_active BOOLEAN DEFAULT true,
    expires_at TIMESTAMP,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- User access cards (many-to-many)
CREATE TABLE IF NOT EXISTS user_access_cards (
    user_id INTEGER NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    access_card_id INTEGER NOT NULL REFERENCES access_cards(id) ON DELETE CASCADE,
    assigned_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (user_id, access_card_id)
);

-- Access card access points (many-to-many)
CREATE TABLE IF NOT EXISTS access_card_access_points (
    access_card_id INTEGER NOT NULL REFERENCES access_cards(id) ON DELETE CASCADE,
    access_point_id INTEGER NOT NULL REFERENCES access_points(id) ON DELETE CASCADE,
    PRIMARY KEY (access_card_id, access_point_id)
);

-- Access logs table
CREATE TABLE IF NOT EXISTS access_logs (
    id SERIAL PRIMARY KEY,
    user_id INTEGER REFERENCES users(id) ON DELETE SET NULL,
    access_card_id INTEGER REFERENCES access_cards(id) ON DELETE SET NULL,
    access_point_id INTEGER NOT NULL REFERENCES access_points(id) ON DELETE CASCADE,
    access_result VARCHAR(20) NOT NULL, -- 'GRANTED' or 'DENIED'
    access_time TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    reason TEXT
);

-- API keys table
CREATE TABLE IF NOT EXISTS api_keys (
    id SERIAL PRIMARY KEY,
    key_value VARCHAR(255) NOT NULL UNIQUE,
    name VARCHAR(100) NOT NULL,
    is_active BOOLEAN DEFAULT true,
    expires_at TIMESTAMP,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    last_used_at TIMESTAMP
);

-- Indexes
CREATE INDEX IF NOT EXISTS idx_users_username ON users(username);
CREATE INDEX IF NOT EXISTS idx_users_email ON users(email);
CREATE INDEX IF NOT EXISTS idx_access_logs_user_id ON access_logs(user_id);
CREATE INDEX IF NOT EXISTS idx_access_logs_access_point_id ON access_logs(access_point_id);
CREATE INDEX IF NOT EXISTS idx_access_logs_access_time ON access_logs(access_time);
CREATE INDEX IF NOT EXISTS idx_access_cards_card_number ON access_cards(card_number);
CREATE INDEX IF NOT EXISTS idx_api_keys_key_value ON api_keys(key_value);


