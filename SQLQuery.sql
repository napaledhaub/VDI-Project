USE VDI

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'transactions')
BEGIN
    DROP TABLE transactions;
END

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'types')
BEGIN
    DROP TABLE types;
END

CREATE TABLE types (
    type_id BIGINT IDENTITY(1,1) PRIMARY KEY,
    name VARCHAR(100),
    div INT,
    low INT,
    mid INT,
    high INT
);

CREATE TABLE transactions (
	id BIGINT IDENTITY(1,1) PRIMARY KEY,
    transaction_id VARCHAR(100) UNIQUE,
    type_id BIGINT,
    reward_points DECIMAL(25,6),
    sub_total DECIMAL(25,6),
    discount DECIMAL(25,6),
    grand_total DECIMAL(25,6),
    FOREIGN KEY (type_id) REFERENCES types(type_id)
);

INSERT INTO types (name, div, low, mid, high)
SELECT 'Silver', 10, 12, 27, 39
UNION ALL
SELECT 'Gold', 25, 25, 34, 52
UNION ALL
SELECT 'Platinum', 50, 35, 50, 68