SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `giipjucc_survey`
--

-- --------------------------------------------------------

--
-- Table structure for table `accounts`
--

CREATE TABLE `accounts` (
  `id` int(11) NOT NULL,
  `username` varchar(16) NOT NULL,
  `password` varchar(16) NOT NULL,
  `name` varchar(26) NOT NULL,
  `surname` varchar(26) NOT NULL,
  `admin` tinyint(4) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `accounts`
--

INSERT INTO `accounts` (`id`, `username`, `password`, `name`, `surname`, `admin`) VALUES
(15, 'Pardus', '1', 'Rüstem', 'Avc?', 1),
(16, 'avci1', '123456', 'Rüstem', 'Avci', 0),
(17, 'avci2', '123456', 'Rüstem', 'Avc?', 0),
(18, 'avci3', '123456', 'Rüstem', 'Avc?', 0),
(19, 'avci4', '123456', 'Rüstem', 'Avc?', 0),
(20, 'avci6', '123456', 'Rüstem', 'Avci', 0);

-- --------------------------------------------------------

--
-- Table structure for table `answers`
--

CREATE TABLE `answers` (
  `id` int(11) NOT NULL,
  `account` int(11) NOT NULL,
  `choice` int(11) NOT NULL,
  `answer` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `answers`
--

INSERT INTO `answers` (`id`, `account`, `choice`, `answer`) VALUES
(634, 15, 90, '0'),
(635, 15, 91, '1'),
(636, 15, 92, '0'),
(637, 15, 93, '0'),
(638, 15, 94, '0'),
(639, 15, 95, '0'),
(640, 15, 96, '0'),
(641, 15, 97, '0'),
(642, 15, 98, '1'),
(643, 15, 99, '0'),
(644, 15, 100, '0'),
(645, 15, 101, '0'),
(646, 15, 102, '0'),
(647, 15, 69, 'Miami'),
(648, 15, 103, '3'),
(649, 15, 81, '1'),
(650, 15, 82, '0'),
(651, 15, 83, '0'),
(652, 15, 84, '0'),
(653, 15, 85, '1'),
(654, 15, 86, '0'),
(655, 15, 87, '0'),
(656, 15, 88, '0'),
(657, 15, 89, '0');

-- --------------------------------------------------------

--
-- Table structure for table `choices`
--

CREATE TABLE `choices` (
  `id` int(11) NOT NULL,
  `question` int(11) NOT NULL,
  `choice_content` text NOT NULL,
  `placement` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `choices`
--

INSERT INTO `choices` (`id`, `question`, `choice_content`, `placement`) VALUES
(69, 26, 'Text', 1),
(81, 27, 'Single', 0),
(82, 27, 'Married', 1),
(83, 27, 'Divorced', 2),
(84, 27, 'Widowed', 3),
(85, 28, '0', 0),
(86, 28, '1', 1),
(87, 28, '2', 2),
(88, 28, '3', 3),
(89, 28, '4+', 4),
(90, 29, 'Under 18', 0),
(91, 29, '18-24', 1),
(92, 29, '25-34', 2),
(93, 29, '35-44', 3),
(94, 29, '45-54', 4),
(95, 29, '55-64', 5),
(96, 29, '64 or older', 6),
(97, 31, 'Daily', 0),
(98, 31, 'Several times a week', 1),
(99, 31, 'Weekly', 2),
(100, 31, 'Bi-weekly', 3),
(101, 31, 'Monthly', 4),
(102, 31, 'Less than once a month', 5),
(103, 32, 'Satisfaction', 1);

-- --------------------------------------------------------

--
-- Table structure for table `questions`
--

CREATE TABLE `questions` (
  `id` int(11) NOT NULL,
  `survey` int(11) NOT NULL,
  `placement` tinyint(4) NOT NULL,
  `question` text NOT NULL,
  `type` tinyint(4) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `questions`
--

INSERT INTO `questions` (`id`, `survey`, `placement`, `question`, `type`) VALUES
(26, 1, 1, 'Which city do you live in?', 1),
(27, 1, 2, 'What is your marital status?', 4),
(28, 1, 3, 'How many children do you have?', 4),
(29, 1, 4, 'What is your age group?', 0),
(31, 1, 4, 'How often do you shop for groceries?', 0),
(32, 1, 5, 'How satisfied are you with Walmart?', 2);

-- --------------------------------------------------------

--
-- Table structure for table `surveys`
--

CREATE TABLE `surveys` (
  `id` int(11) NOT NULL,
  `title` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `surveys`
--

INSERT INTO `surveys` (`id`, `title`) VALUES
(1, 'What are your supermarket preferences?'),
(2, 'The Best Video Games of 2022 by Category'),
(3, 'Most Preferred Browsers');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `accounts`
--
ALTER TABLE `accounts`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `answers`
--
ALTER TABLE `answers`
  ADD PRIMARY KEY (`id`),
  ADD KEY `FK_answers_accounts` (`account`),
  ADD KEY `FK_answers_choices` (`choice`);

--
-- Indexes for table `choices`
--
ALTER TABLE `choices`
  ADD PRIMARY KEY (`id`),
  ADD KEY `FK_choices_questions` (`question`);

--
-- Indexes for table `questions`
--
ALTER TABLE `questions`
  ADD PRIMARY KEY (`id`),
  ADD KEY `FK_questions_giipjucc_survey` (`survey`);

--
-- Indexes for table `surveys`
--
ALTER TABLE `surveys`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `accounts`
--
ALTER TABLE `accounts`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;

--
-- AUTO_INCREMENT for table `answers`
--
ALTER TABLE `answers`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=658;

--
-- AUTO_INCREMENT for table `choices`
--
ALTER TABLE `choices`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=104;

--
-- AUTO_INCREMENT for table `questions`
--
ALTER TABLE `questions`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=33;

--
-- AUTO_INCREMENT for table `surveys`
--
ALTER TABLE `surveys`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `answers`
--
ALTER TABLE `answers`
  ADD CONSTRAINT `FK_answers_accounts` FOREIGN KEY (`account`) REFERENCES `accounts` (`id`),
  ADD CONSTRAINT `FK_answers_choices` FOREIGN KEY (`choice`) REFERENCES `choices` (`id`),
  ADD CONSTRAINT `answers_ibfk_1` FOREIGN KEY (`account`) REFERENCES `accounts` (`id`),
  ADD CONSTRAINT `answers_ibfk_2` FOREIGN KEY (`choice`) REFERENCES `choices` (`id`);

--
-- Constraints for table `choices`
--
ALTER TABLE `choices`
  ADD CONSTRAINT `FK_choices_questions` FOREIGN KEY (`question`) REFERENCES `questions` (`id`),
  ADD CONSTRAINT `choices_ibfk_1` FOREIGN KEY (`question`) REFERENCES `questions` (`id`);

--
-- Constraints for table `questions`
--
ALTER TABLE `questions`
  ADD CONSTRAINT `FK_questions_giipjucc_survey` FOREIGN KEY (`survey`) REFERENCES `surveys` (`id`),
  ADD CONSTRAINT `questions_ibfk_1` FOREIGN KEY (`survey`) REFERENCES `surveys` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
