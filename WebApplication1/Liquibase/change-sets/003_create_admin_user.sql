--liquibase formatted sql

--changeset student:003_create_admin_user
--comment: Create default admin user (password: admin123)

-- Insert admin user (password hash for "admin123")
-- Hash is calculated as: Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes("admin123")))
INSERT INTO users (username, email, password_hash, first_name, last_name, is_active, created_at, updated_at)
VALUES (
    'admin',
    'admin@example.com',
    'JAvlGPq9JyTdtvBO6x2llnRI1+gxwIyPqCKAn3THIKk=', -- SHA256 hash of "admin123"
    'Admin',
    'User',
    true,
    CURRENT_TIMESTAMP,
    CURRENT_TIMESTAMP
)
ON CONFLICT (username) DO NOTHING;

-- Assign Admin role to admin user
INSERT INTO user_roles (user_id, role_id)
SELECT u.id, r.id
FROM users u, roles r
WHERE u.username = 'admin' AND r.name = 'Admin'
ON CONFLICT DO NOTHING;

