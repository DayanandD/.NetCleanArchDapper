-- Create VMS schema
CREATE SCHEMA IF NOT EXISTS vms;
SET search_path TO vms;

-- Enable UUID extension
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- UserMaster table
CREATE TABLE NETCleanArchUserMaster (
    UserId UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    Email VARCHAR(255) NOT NULL UNIQUE,
    PasswordHash TEXT NOT NULL,
    PasswordSalt TEXT NOT NULL,
    PhoneNumber VARCHAR(20),
    Department VARCHAR(100) NOT NULL,
    EmployeeId VARCHAR(50) NOT NULL UNIQUE,
    ExtNumber VARCHAR(20),
    Role INTEGER NOT NULL DEFAULT 5, -- Default to Employee
    IsActive BOOLEAN NOT NULL DEFAULT true,
    CreatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP,
    LastLoginAt TIMESTAMP,
    RefreshToken TEXT,
    RefreshTokenExpiry TIMESTAMP,
    ResetPasswordToken TEXT,
    ResetPasswordExpiry TIMESTAMP
);

-- RoleMaster table
CREATE TABLE NETCleanArchRoleMaster (
    RoleId UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    Name VARCHAR(50) NOT NULL UNIQUE,
    Description VARCHAR(255),
    Permissions JSONB NOT NULL DEFAULT '[]',
    IsActive BOOLEAN NOT NULL DEFAULT true,
    CreatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- MenuMaster table
CREATE TABLE NETCleanArchMenuMaster (
    MenuId UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    Name VARCHAR(100) NOT NULL,
    Description VARCHAR(255),
    Path VARCHAR(255),
    Icon VARCHAR(100),
    ParentMenuId UUID REFERENCES NETCleanArchMenuMaster(MenuId),
    DisplayOrder INTEGER NOT NULL DEFAULT 0,
    IsActive BOOLEAN NOT NULL DEFAULT true,
    CreatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- RoleMenuMapping table
CREATE TABLE NETCleanArchRoleMenuMapping (
    RoleMenuMappingId UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    RoleId UUID NOT NULL REFERENCES NETCleanArchRoleMaster(RoleId),
    MenuId UUID NOT NULL REFERENCES NETCleanArchMenuMaster(MenuId),
    CanRead BOOLEAN NOT NULL DEFAULT false,
    CanWrite BOOLEAN NOT NULL DEFAULT false,
    CanDelete BOOLEAN NOT NULL DEFAULT false,
    CreatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(RoleId, MenuId)
);

-- UserRoleMapping table
CREATE TABLE NETCleanArchUserRoleMapping (
    UserRoleMappingId UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    UserId UUID NOT NULL REFERENCES NETCleanArchUserMaster(UserId),
    RoleId UUID NOT NULL REFERENCES NETCleanArchRoleMaster(RoleId),
    CreatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(UserId, RoleId)
);

-- VisitorMaster table
CREATE TABLE NETCleanArchVisitorMaster (
    VisitorId UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    Name VARCHAR(200) NOT NULL,
    ContactNumber VARCHAR(20) NOT NULL,
    PhotoUrl VARCHAR(500),
    IdType VARCHAR(50),
    IdDocumentUrl VARCHAR(500),
    IsVendor BOOLEAN DEFAULT false,
    VendorId UUID,
    CreatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CreatedBy UUID REFERENCES NETCleanArchUserMaster(UserId)
);

-- VendorMaster table
CREATE TABLE NETCleanArchVendorMaster (
    VendorId UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    CompanyName VARCHAR(200) NOT NULL,
    ContactPerson VARCHAR(200) NOT NULL,
    ContactNumber VARCHAR(20) NOT NULL,
    Email VARCHAR(255),
    Address TEXT,
    VerifiedDocs JSONB,
    IsActive BOOLEAN NOT NULL DEFAULT true,
    CreatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CreatedBy UUID REFERENCES NETCleanArchUserMaster(UserId)
);

-- VisitorVisit table
CREATE TABLE NETCleanArchVisitorVisit (
    VisitId UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    VisitorId UUID NOT NULL REFERENCES NETCleanArchVisitorMaster(VisitorId),
    VisiteeId UUID NOT NULL REFERENCES NETCleanArchUserMaster(UserId),
    VisiteeFirstName VARCHAR(100) NOT NULL,
    VisiteeLastName VARCHAR(100) NOT NULL,
    CheckInTime TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CheckOutTime TIMESTAMP,
    Purpose VARCHAR(500) NOT NULL,
    Status VARCHAR(50) NOT NULL DEFAULT 'Waiting',
    PhotoCaptureUrl VARCHAR(500),
    DocumentUrls JSONB,
    EntrySource VARCHAR(50) NOT NULL,
    CreatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- VendorVisitor table (for authorized vendor visitors)
CREATE TABLE NETCleanArchVendorVisitor (
    VendorVisitorId UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    VendorId UUID NOT NULL REFERENCES NETCleanArchVendorMaster(VendorId),
    VisitorId UUID NOT NULL REFERENCES NETCleanArchVisitorMaster(VisitorId),
    AuthorizationStart TIMESTAMP NOT NULL,
    AuthorizationEnd TIMESTAMP NOT NULL,
    Recurring BOOLEAN NOT NULL DEFAULT false,
    RecurringPattern VARCHAR(50), -- 'Daily', 'Weekly', 'Monthly'
    IsActive BOOLEAN NOT NULL DEFAULT true,
    CreatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CreatedBy UUID REFERENCES NETCleanArchUserMaster(UserId)
);

-- AuditLog table
CREATE TABLE NETCleanArchAuditLog (
    AuditId UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    EntityType VARCHAR(100) NOT NULL,
    EntityId UUID NOT NULL,
    Action VARCHAR(50) NOT NULL, -- 'Create', 'Update', 'Delete', 'CheckIn', 'CheckOut'
    ChangedBy UUID REFERENCES NETCleanArchUserMaster(UserId),
    Changes JSONB,
    Timestamp TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    IpAddress VARCHAR(45),
    UserAgent TEXT
);

-- Notifications table
CREATE TABLE NETCleanArchNotifications (
    NotificationId UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    ToUserId UUID NOT NULL REFERENCES NETCleanArchUserMaster(UserId),
    Type VARCHAR(50) NOT NULL, -- 'Email', 'SMS', 'Push', 'System'
    Title VARCHAR(255) NOT NULL,
    Message TEXT NOT NULL,
    Payload JSONB,
    Status VARCHAR(20) NOT NULL DEFAULT 'Pending', -- 'Pending', 'Sent', 'Failed'
    SentAt TIMESTAMP,
    AttemptCount INTEGER NOT NULL DEFAULT 0,
    MaxAttempts INTEGER NOT NULL DEFAULT 3,
    CreatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- ReportsCache table for precomputed aggregates
CREATE TABLE NETCleanArchReportsCache (
    ReportCacheId UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    ReportType VARCHAR(100) NOT NULL,
    ReportDate DATE NOT NULL,
    Data JSONB NOT NULL,
    GeneratedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(ReportType, ReportDate)
);

-- OCRJobs table for background processing
CREATE TABLE NETCleanArchOCRJobs (
    OcrJobId UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    VisitId UUID REFERENCES NETCleanArchVisitorVisit(VisitId),
    DocumentUrl VARCHAR(500) NOT NULL,
    DocumentType VARCHAR(50) NOT NULL, -- 'VisitingCard', 'IDDocument'
    Status VARCHAR(20) NOT NULL DEFAULT 'Pending', -- 'Pending', 'Processing', 'Completed', 'Failed'
    RawText TEXT,
    ParsedData JSONB,
    ConfidenceScore DECIMAL(5,4),
    ErrorMessage TEXT,
    CreatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ProcessedAt TIMESTAMP,
    AttemptCount INTEGER NOT NULL DEFAULT 0
);

-- Outbox table for reliable messaging
CREATE TABLE NETCleanArchOutbox (
    OutboxId UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    EventType VARCHAR(255) NOT NULL,
    EventData JSONB NOT NULL,
    CreatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ProcessedAt TIMESTAMP,
    ErrorMessage TEXT,
    AttemptCount INTEGER NOT NULL DEFAULT 0
);

-- =============================================
-- CREATE INDEXES FOR PERFORMANCE
-- =============================================

-- UserMaster indexes
CREATE INDEX IX_UserMaster_Email ON NETCleanArchUserMaster(Email);
CREATE INDEX IX_UserMaster_EmployeeId ON NETCleanArchUserMaster(EmployeeId);
CREATE INDEX IX_UserMaster_RefreshToken ON NETCleanArchUserMaster(RefreshToken);
CREATE INDEX IX_UserMaster_ResetPasswordToken ON NETCleanArchUserMaster(ResetPasswordToken);
CREATE INDEX IX_UserMaster_IsActive ON NETCleanArchUserMaster(IsActive);
CREATE INDEX IX_UserMaster_Role ON NETCleanArchUserMaster(Role);

-- VisitorMaster indexes
CREATE INDEX IX_VisitorMaster_ContactNumber ON NETCleanArchVisitorMaster(ContactNumber);
CREATE INDEX IX_VisitorMaster_IsVendor ON NETCleanArchVisitorMaster(IsVendor);
CREATE INDEX IX_VisitorMaster_VendorId ON NETCleanArchVisitorMaster(VendorId);
CREATE INDEX IX_VisitorMaster_CreatedAt ON NETCleanArchVisitorMaster(CreatedAt);

-- VisitorVisit indexes
CREATE INDEX IX_VisitorVisit_Status ON NETCleanArchVisitorVisit(Status);
CREATE INDEX IX_VisitorVisit_CheckInTime ON NETCleanArchVisitorVisit(CheckInTime);
CREATE INDEX IX_VisitorVisit_CheckOutTime ON NETCleanArchVisitorVisit(CheckOutTime);
CREATE INDEX IX_VisitorVisit_VisitorId ON NETCleanArchVisitorVisit(VisitorId);
CREATE INDEX IX_VisitorVisit_VisiteeId ON NETCleanArchVisitorVisit(VisiteeId);
CREATE INDEX IX_VisitorVisit_EntrySource ON NETCleanArchVisitorVisit(EntrySource);
CREATE INDEX IX_VisitorVisit_CreatedAt ON NETCleanArchVisitorVisit(CreatedAt);

-- VendorMaster indexes
CREATE INDEX IX_VendorMaster_CompanyName ON NETCleanArchVendorMaster(CompanyName);
CREATE INDEX IX_VendorMaster_ContactNumber ON NETCleanArchVendorMaster(ContactNumber);
CREATE INDEX IX_VendorMaster_IsActive ON NETCleanArchVendorMaster(IsActive);

-- VendorVisitor indexes
CREATE INDEX IX_VendorVisitor_VendorId ON NETCleanArchVendorVisitor(VendorId);
CREATE INDEX IX_VendorVisitor_VisitorId ON NETCleanArchVendorVisitor(VisitorId);
CREATE INDEX IX_VendorVisitor_AuthorizationStart ON NETCleanArchVendorVisitor(AuthorizationStart);
CREATE INDEX IX_VendorVisitor_AuthorizationEnd ON NETCleanArchVendorVisitor(AuthorizationEnd);
CREATE INDEX IX_VendorVisitor_IsActive ON NETCleanArchVendorVisitor(IsActive);

-- AuditLog indexes
CREATE INDEX IX_AuditLog_EntityType_EntityId ON NETCleanArchAuditLog(EntityType, EntityId);
CREATE INDEX IX_AuditLog_ChangedBy ON NETCleanArchAuditLog(ChangedBy);
CREATE INDEX IX_AuditLog_Timestamp ON NETCleanArchAuditLog(Timestamp);

-- Notifications indexes
CREATE INDEX IX_Notifications_ToUserId ON NETCleanArchNotifications(ToUserId);
CREATE INDEX IX_Notifications_Status ON NETCleanArchNotifications(Status);
CREATE INDEX IX_Notifications_CreatedAt ON NETCleanArchNotifications(CreatedAt);

-- ReportsCache indexes
CREATE INDEX IX_ReportsCache_ReportType_ReportDate ON NETCleanArchReportsCache(ReportType, ReportDate);

-- OCRJobs indexes
CREATE INDEX IX_OCRJobs_VisitId ON NETCleanArchOCRJobs(VisitId);
CREATE INDEX IX_OCRJobs_Status ON NETCleanArchOCRJobs(Status);
CREATE INDEX IX_OCRJobs_CreatedAt ON NETCleanArchOCRJobs(CreatedAt);

-- Outbox indexes
CREATE INDEX IX_Outbox_ProcessedAt ON NETCleanArchOutbox(ProcessedAt);
CREATE INDEX IX_Outbox_CreatedAt ON NETCleanArchOutbox(CreatedAt);

-- =============================================
-- INSERT DEFAULT DATA
-- =============================================

-- Insert default roles
INSERT INTO NETCleanArchRoleMaster (RoleId, Name, Description, Permissions) VALUES
('11111111-1111-1111-1111-111111111111', 'SuperAdmin', 'Super Administrator', '["all"]'),
('22222222-2222-2222-2222-222222222222', 'Admin', 'Administrator', '["users.read", "users.write", "visitors.read", "visitors.write", "reports.read", "vendors.read", "vendors.write"]'),
('33333333-3333-3333-3333-333333333333', 'Guard', 'Security Guard', '["visitors.read", "visitors.write", "checkin.write", "checkout.write", "vendors.read"]'),
('44444444-4444-4444-4444-444444444444', 'Receptionist', 'Reception Staff', '["visitors.read", "visitors.write", "checkin.write", "checkout.write", "reports.read", "vendors.read"]'),
('55555555-5555-5555-5555-555555555555', 'Employee', 'Regular Employee', '["visitors.read", "profile.read"]');

-- Insert menu items
INSERT INTO NETCleanArchMenuMaster (MenuId, Name, Description, Path, Icon, ParentMenuId, DisplayOrder) VALUES
-- Main menus
('10000000-0000-0000-0000-000000000001', 'Dashboard', 'Main Dashboard', '/dashboard', 'Dashboard', NULL, 1),
('10000000-0000-0000-0000-000000000002', 'Visitor Management', 'Manage Visitors', '/visitors', 'People', NULL, 2),
('10000000-0000-0000-0000-000000000003', 'Lobby', 'Live Lobby View', '/lobby', 'MeetingRoom', NULL, 3),
('10000000-0000-0000-0000-000000000004', 'Vendors', 'Vendor Management', '/vendors', 'Business', NULL, 4),
('10000000-0000-0000-0000-000000000005', 'Reports', 'View Reports', '/reports', 'Assessment', NULL, 5),
('10000000-0000-0000-0000-000000000006', 'Admin', 'Administration', '/admin', 'AdminPanelSettings', NULL, 6),

-- Sub-menus for Admin
('20000000-0000-0000-0000-000000000001', 'Users', 'User Management', '/admin/users', 'Person', '10000000-0000-0000-0000-000000000006', 1),
('20000000-0000-0000-0000-000000000002', 'Roles', 'Role Management', '/admin/roles', 'AdminPanelSettings', '10000000-0000-0000-0000-000000000006', 2),
('20000000-0000-0000-0000-000000000003', 'Audit Logs', 'System Audit Logs', '/admin/audit-logs', 'History', '10000000-0000-0000-0000-000000000006', 3);

-- Insert default admin user (password: Admin123!)
INSERT INTO NETCleanArchUserMaster (
    UserId, FirstName, LastName, Email, PasswordHash, PasswordSalt, 
    Department, EmployeeId, Role, PhoneNumber
) VALUES (
    '11111111-1111-1111-1111-111111111111',
    'System',
    'Administrator',
    'admin@NETCleanArchcom',
    'gz1mJ5J4Q6hK5vY7w8bW0R7sS4jK7lN3tA2mF1pL9eE3qX6rC8vB5nH2yM4dG7wV0',
    'T3sT5aLtK3y1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789+/',
    'IT',
    'ADMIN001',
    1,
    '+1234567890'
);

-- Assign SuperAdmin role to admin user
INSERT INTO NETCleanArchUserRoleMapping (UserRoleMappingId, UserId, RoleId) VALUES
(uuid_generate_v4(), '11111111-1111-1111-1111-111111111111', '11111111-1111-1111-1111-111111111111');

-- Insert role-menu mappings for SuperAdmin (all access)
INSERT INTO NETCleanArchRoleMenuMapping (RoleMenuMappingId, RoleId, MenuId, CanRead, CanWrite, CanDelete)
SELECT 
    uuid_generate_v4(),
    '11111111-1111-1111-1111-111111111111',
    MenuId,
    true, true, true
FROM NETCleanArchMenuMaster;

-- Insert role-menu mappings for Admin
INSERT INTO NETCleanArchRoleMenuMapping (RoleMenuMappingId, RoleId, MenuId, CanRead, CanWrite, CanDelete) VALUES
-- Dashboard
(uuid_generate_v4(), '22222222-2222-2222-2222-222222222222', '10000000-0000-0000-0000-000000000001', true, true, false),
-- Visitor Management
(uuid_generate_v4(), '22222222-2222-2222-2222-222222222222', '10000000-0000-0000-0000-000000000002', true, true, true),
-- Lobby
(uuid_generate_v4(), '22222222-2222-2222-2222-222222222222', '10000000-0000-0000-0000-000000000003', true, true, false),
-- Vendors
(uuid_generate_v4(), '22222222-2222-2222-2222-222222222222', '10000000-0000-0000-0000-000000000004', true, true, true),
-- Reports
(uuid_generate_v4(), '22222222-2222-2222-2222-222222222222', '10000000-0000-0000-0000-000000000005', true, false, false);

-- Insert role-menu mappings for Guard
INSERT INTO NETCleanArchRoleMenuMapping (RoleMenuMappingId, RoleId, MenuId, CanRead, CanWrite, CanDelete) VALUES
-- Dashboard
(uuid_generate_v4(), '33333333-3333-3333-3333-333333333333', '10000000-0000-0000-0000-000000000001', true, false, false),
-- Visitor Management
(uuid_generate_v4(), '33333333-3333-3333-3333-333333333333', '10000000-0000-0000-0000-000000000002', true, true, false),
-- Lobby
(uuid_generate_v4(), '33333333-3333-3333-3333-333333333333', '10000000-0000-0000-0000-000000000003', true, true, false),
-- Vendors
(uuid_generate_v4(), '33333333-3333-3333-3333-333333333333', '10000000-0000-0000-0000-000000000004', true, false, false);

-- Insert sample vendor
INSERT INTO NETCleanArchVendorMaster (VendorId, CompanyName, ContactPerson, ContactNumber, Email, Address) VALUES
('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'Tech Solutions Inc.', 'John Smith', '+1234567890', 'john@techsolutions.com', '123 Business Ave, City, State 12345');

-- Insert sample visitor
INSERT INTO NETCleanArchVisitorMaster (VisitorId, Name, ContactNumber, IsVendor, VendorId) VALUES
('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 'Jane Doe', '+1987654321', true, 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa');

-- =============================================
-- CREATE FUNCTIONS AND TRIGGERS
-- =============================================

-- Function to update UpdatedAt timestamp
CREATE OR REPLACE FUNCTION NETCleanArchupdate_updated_at_column()
RETURNS TRIGGER AS $$
BEGIN
    NEW.UpdatedAt = CURRENT_TIMESTAMP;
    RETURN NEW;
END;
$$ language 'plpgsql';

-- Create triggers for UpdatedAt
CREATE TRIGGER update_visitor_master_updated_at 
    BEFORE UPDATE ON NETCleanArchVisitorMaster 
    FOR EACH ROW EXECUTE FUNCTION NETCleanArchupdate_updated_at_column();

CREATE TRIGGER update_vendor_master_updated_at 
    BEFORE UPDATE ON NETCleanArchVendorMaster 
    FOR EACH ROW EXECUTE FUNCTION NETCleanArchupdate_updated_at_column();

CREATE TRIGGER update_visitor_visit_updated_at 
    BEFORE UPDATE ON NETCleanArchVisitorVisit 
    FOR EACH ROW EXECUTE FUNCTION NETCleanArchupdate_updated_at_column();

CREATE TRIGGER update_vendor_visitor_updated_at 
    BEFORE UPDATE ON NETCleanArchVendorVisitor 
    FOR EACH ROW EXECUTE FUNCTION NETCleanArchupdate_updated_at_column();

-- Function to auto-populate visitee names
CREATE OR REPLACE FUNCTION NETCleanArchpopulate_visitee_names()
RETURNS TRIGGER AS $$
BEGIN
    SELECT FirstName, LastName 
    INTO NEW.VisiteeFirstName, NEW.VisiteeLastName
    FROM NETCleanArchUserMaster 
    WHERE UserId = NEW.VisiteeId;
    
    RETURN NEW;
END;
$$ language 'plpgsql';

CREATE TRIGGER populate_visitee_names_trigger
    BEFORE INSERT ON NETCleanArchVisitorVisit
    FOR EACH ROW EXECUTE FUNCTION NETCleanArchpopulate_visitee_names();

-- Function for audit logging
CREATE OR REPLACE FUNCTION NETCleanArchlog_audit_event()
RETURNS TRIGGER AS $$
DECLARE
    old_data JSONB;
    new_data JSONB;
    changes JSONB;
BEGIN
    IF TG_OP = 'INSERT' THEN
        new_data = row_to_json(NEW);
        INSERT INTO NETCleanArchAuditLog (EntityType, EntityId, Action, ChangedBy, Changes)
        VALUES (TG_TABLE_NAME, NEW.VisitorId, 'Create', NEW.CreatedBy, new_data);
    ELSIF TG_OP = 'UPDATE' THEN
        old_data = row_to_json(OLD);
        new_data = row_to_json(NEW);
        changes = jsonb_build_object(
            'old', old_data,
            'new', new_data
        );
        INSERT INTO NETCleanArchAuditLog (EntityType, EntityId, Action, ChangedBy, Changes)
        VALUES (TG_TABLE_NAME, NEW.VisitorId, 'Update', NEW.CreatedBy, changes);
    ELSIF TG_OP = 'DELETE' THEN
        old_data = row_to_json(OLD);
        INSERT INTO NETCleanArchAuditLog (EntityType, EntityId, Action, ChangedBy, Changes)
        VALUES (TG_TABLE_NAME, OLD.VisitorId, 'Delete', OLD.CreatedBy, old_data);
    END IF;
    RETURN COALESCE(NEW, OLD);
END;
$$ language 'plpgsql';

-- Create audit triggers for key tables
CREATE TRIGGER audit_visitor_master
    AFTER INSERT OR UPDATE OR DELETE ON NETCleanArchVisitorMaster
    FOR EACH ROW EXECUTE FUNCTION NETCleanArchlog_audit_event();

CREATE TRIGGER audit_visitor_visit
    AFTER INSERT OR UPDATE OR DELETE ON NETCleanArchVisitorVisit
    FOR EACH ROW EXECUTE FUNCTION NETCleanArchlog_audit_event();

-- Grant permissions to application user
GRANT USAGE ON SCHEMA vms TO vms_user;
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA vms TO vms_user;
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA vms TO vms_user;
GRANT EXECUTE ON ALL FUNCTIONS IN SCHEMA vms TO vms_user;