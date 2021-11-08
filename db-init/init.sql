DROP TABLE IF EXISTS `user`;
CREATE TABLE `user` (
    `id` mediumint(9) NOT NULL AUTO_INCREMENT,
    `name` varchar(255) NOT NULL,
    `emailAddress` varchar(255) NOT NULL,
    `monthlySalary` double(8,2) NOT NULL,
    `monthlyExpenses` double(9,0) NOT NULL,
    PRIMARY KEY(`id`)
);

DROP TABLE IF EXISTS `account`;
CREATE TABLE `account` (
    `id` mediumint(9) NOT NULL AUTO_INCREMENT,
    `userId` mediumint(9) NOT NULL,
	`emailAddress` varchar(255) NOT NULL,
    `accountNumber` varchar(255) NOT NULL,
    `accountName` varchar(255) NOT NULL,
	`creditBalance` double(6,2) NOT NULL,
    PRIMARY KEY(`id`)
);