--liquibase formatted sql

--changeset student:002_initial_data
--comment: Initial data for roles and permissions

-- Insert roles
INSERT INTO roles (name, description) VALUES
('Admin', 'Administrator with full access'),
('Manager', 'Manager with limited administrative access'),
('User', 'Regular user with read-only access')
ON CONFLICT (name) DO NOTHING;

-- Insert permissions
INSERT INTO permissions (name, description) VALUES
('users.read', 'Read users'),
('users.create', 'Create users'),
('users.update', 'Update users'),
('users.delete', 'Delete users'),
('access_points.read', 'Read access points'),
('access_points.create', 'Create access points'),
('access_points.update', 'Update access points'),
('access_points.delete', 'Delete access points'),
('access_cards.read', 'Read access cards'),
('access_cards.create', 'Create access cards'),
('access_cards.update', 'Update access cards'),
('access_cards.delete', 'Delete access cards'),
('access_logs.read', 'Read access logs')
ON CONFLICT (name) DO NOTHING;

-- Assign permissions to Admin role
INSERT INTO role_permissions (role_id, permission_id)
SELECT r.id, p.id
FROM roles r, permissions p
WHERE r.name = 'Admin'
ON CONFLICT DO NOTHING;

-- Assign permissions to Manager role
INSERT INTO role_permissions (role_id, permission_id)
SELECT r.id, p.id
FROM roles r, permissions p
WHERE r.name = 'Manager' AND p.name IN (
    'users.read', 'users.create', 'users.update',
    'access_points.read', 'access_points.create', 'access_points.update',
    'access_cards.read', 'access_cards.create', 'access_cards.update',
    'access_logs.read'
)
ON CONFLICT DO NOTHING;

-- Assign permissions to User role
INSERT INTO role_permissions (role_id, permission_id)
SELECT r.id, p.id
FROM roles r, permissions p
WHERE r.name = 'User' AND p.name IN (
    'users.read',
    'access_points.read',
    'access_cards.read',
    'access_logs.read'
)
ON CONFLICT DO NOTHING;


