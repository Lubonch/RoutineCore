-- =====================================================================
-- DATABASE SETUP & SEED SCRIPT FOR ROUTINECORE
-- Platform: PostgreSQL
-- Author: GitHub Copilot (using Gemini 3.5 Flash)
-- Date: May 29, 2026
-- Description: Sets up the full relational database structure and populates
--              it with high-quality demo data for testing
--              operating schedules, punches, and absence pipelines.
-- =====================================================================

-- 1. Create Tables (Using lowercase names mapping to Fluent NHibernate Configuration)

-- Employee Accounts Setup
CREATE TABLE IF NOT EXISTS employees (
    id UUID PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    role VARCHAR(100) NOT NULL,
    employee_code VARCHAR(100) UNIQUE NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE
);

-- Operational Reference Tasks (Studio shifts, Control rooms, etc.)
CREATE TABLE IF NOT EXISTS project_tasks (
    id UUID PRIMARY KEY,
    description VARCHAR(255) NOT NULL,
    project_code VARCHAR(100) NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE
);

-- Staff Scheduling & Operational Roster
CREATE TABLE IF NOT EXISTS schedules (
    id UUID PRIMARY KEY,
    employee_id UUID NOT NULL,
    start_time TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    end_time TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    project_task_id UUID NOT NULL,
    status VARCHAR(100) NOT NULL DEFAULT 'Scheduled',
    CONSTRAINT fk_schedules_employees FOREIGN KEY (employee_id) REFERENCES employees(id) ON DELETE CASCADE,
    CONSTRAINT fk_schedules_tasks FOREIGN KEY (project_task_id) REFERENCES project_tasks(id) ON DELETE CASCADE
);

-- Real-time Biometric or Web Punches
CREATE TABLE IF NOT EXISTS punches (
    id UUID PRIMARY KEY,
    employee_id UUID NOT NULL,
    punch_time TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    direction VARCHAR(50) NOT NULL,
    device_code VARCHAR(100) NOT NULL,
    processed BOOLEAN NOT NULL DEFAULT FALSE,
    CONSTRAINT fk_punches_employees FOREIGN KEY (employee_id) REFERENCES employees(id) ON DELETE CASCADE
);

-- Time-off, Absence, and Leave Authorizations
CREATE TABLE IF NOT EXISTS absences (
    id UUID PRIMARY KEY,
    employee_id UUID NOT NULL,
    start_date TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    end_date TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    reason TEXT NOT NULL,
    authorized BOOLEAN NOT NULL DEFAULT FALSE,
    approved_by VARCHAR(255) NULL,
    CONSTRAINT fk_absences_employees FOREIGN KEY (employee_id) REFERENCES employees(id) ON DELETE CASCADE
);

-- 2. Clear Existing Records (Ensures fresh start during development executions)
TRUNCATE TABLE absences, punches, schedules, project_tasks, employees CASCADE;

-- 3. Seed Demo Data

-- Insert standard corporate accounts
INSERT INTO employees (id, name, email, role, employee_code, is_active) VALUES
('00000000-0000-0000-0000-000000000001', 'Admin Staff', 'admin@routinecore.com', 'Admin', 'EMP001', TRUE),
('00000000-0000-0000-0000-000000000002', 'Standard Operator', 'operator@routinecore.com', 'Operator', 'EMP002', TRUE),
('00000000-0000-0000-0000-000000000003', 'Operations Manager', 'manager@routinecore.com', 'Manager', 'EMP003', TRUE),
('00000000-0000-0000-0000-000000000004', 'Studio Supervisor', 'supervisor@routinecore.com', 'Operator', 'EMP004', TRUE);

-- Insert operational tasks representing production assignments
INSERT INTO project_tasks (id, description, project_code, is_active) VALUES
('11111111-1111-1111-1111-111111111111', 'Master Control Room (MCR)', 'CONTROL-ROOM', TRUE),
('22222222-2222-2222-2222-222222222222', 'News Studio Production Shift', 'NEWS-PROD', TRUE),
('33333333-3333-3333-3333-333333333333', 'Live Broadcast Sound Board Mixing', 'AUDIO-ENG', TRUE),
('44444444-4444-4444-4444-444444444444', 'Camera Operator & Studio Floor Management', 'CAMERA-CREW', TRUE);

-- Insert active planning schedules
INSERT INTO schedules (id, employee_id, start_time, end_time, project_task_id, status) VALUES
('a0000000-0000-0000-0000-000000000001', '00000000-0000-0000-0000-000000000002', '2026-05-29 08:00:00', '2026-05-29 16:00:00', '11111111-1111-1111-1111-111111111111', 'Scheduled'),
('a0000000-0000-0000-0000-000000000002', '00000000-0000-0000-0000-000000000004', '2026-05-29 16:00:00', '2026-05-30 00:00:00', '44444444-4444-4444-4444-444444444444', 'Scheduled'),
('a0000000-0000-0000-0000-000000000003', '00000000-0000-0000-0000-000000000001', '2026-05-29 09:00:00', '2026-05-29 17:00:00', '22222222-2222-2222-2222-222222222222', 'Scheduled');

-- Insert initial punches
INSERT INTO punches (id, employee_id, punch_time, direction, device_code, processed) VALUES
('b0000000-0000-0000-0000-000000000001', '00000000-0000-0000-0000-000000000002', '2026-05-29 07:55:12', 'In', 'Main Entrance Biometric', TRUE),
('b0000000-0000-0000-0000-000000000002', '00000000-0000-0000-0000-000000000002', '2026-05-29 16:02:45', 'Out', 'Main Entrance Biometric', TRUE);

-- Insert pending absence leave request
INSERT INTO absences (id, employee_id, start_date, end_date, reason, authorized, approved_by) VALUES
('c0000000-0000-0000-0000-000000000001', '00000000-0000-0000-0000-000000000002', '2026-06-05 00:00:00', '2026-06-12 23:59:59', 'Annual Vacations - Travel abroad', FALSE, NULL);

-- =====================================================================
-- SCRIPT END
-- =====================================================================
