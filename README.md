# Advent of Code Template Repository

## 🌟 What is Advent of Code?

[Advent of Code](https://adventofcode.com/) is an annual event during December that provides daily programming puzzles. It's a fun and engaging way to challenge your coding skills, learn new techniques, and compete with friends or colleagues.

## 📂 What is this repository about?

This repository is a template designed to simplify solving Advent of Code puzzles. It includes:
- A CLI tool to manage and streamline your workflow.
- A dedicated space for your solutions.
- Tests based on Advent of Code puzzle samples to validate your code.

Whether you're a beginner or a seasoned Advent of Code participant, this repo helps you stay organized and efficient.

## 🚀 How to start using this repository

1. **Clone the repository**:
   ```bash
   git clone git@github.com:maurobellati/adventofcode.git
   cd adventofcode
   ```

2. **Clean the solutions**: If you'd like to start fresh, remove any example solutions provided in the `Solutions` project.

3. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

4. **Build the solution**:
   ```bash
   dotnet build
   ```

## 🛠️ Projects Overview

This repository contains three projects:

1. **CLI Tool**: The main tool to manage Advent of Code tasks, including initializing problems, running solvers, and uploading answers.
2. **Solutions**: A project where you write your solutions for each puzzle.
3. **Tests**: A test project to validate your solutions against the samples provided in the Advent of Code puzzles.

---

## 🔧 Using the CLI Tool

The CLI tool simplifies common tasks for solving Advent of Code puzzles. You can use it directly from the `src/AdventOfCode.Solutions` directory. There's no need to run it from the CLI project, as the `Solutions` project links the CLI for ease of use.

### ⚙️ Initial Setup

Before using the CLI for the first time, configure it by running:

```bash
cd src/AdventOfCode.Solutions
dotnet run -- config
```

This command prompts you to set up the following parameters:

- **Namespace template**: Defines the default namespace for your solution classes (e.g., `AdventOfCode.Y{Year}.Day{Day}`).
- **Class name template**: Specifies the default naming convention for your solution classes (e.g., `Y{Year}Day{Day}`).
- **Class path template**: Determines where solution files will be saved (e.g., `Y{Year}/Day{Day}`).
- **Session cookie**: Required to download inputs and submit answers.

An example interaction might look like this:

```bash
Enter the default namespace template: (AdventOfCode.Y{Year}.Day{Day}): AdventOfCode.Y{Year}.Day{Day}
Enter the default class name template: (Y{Year}Day{Day}): Y{Year}Day{Day}
Enter the default class path template: (Y{Year}/Day{Day}): Y{Year}/Day{Day}
Enter the session cookie: [Your session cookie from Advent of Code]
Configuration file created successfully.
```

This will generate a `.aocconfig` file in the `src/AdventOfCode.Solutions` directory to save your preferences.

---

### 1️⃣ Initialize a Day

To set up the necessary files for a specific year and day, use:

```bash
cd src/AdventOfCode.Solutions
dotnet run -- init 2024 1
```

This will:
- Create a solution file based on your configuration in the `Solutions` project.
- Generate a test file in the `Tests` project.
- Download and save the input file for the puzzle.

---

### 2️⃣ Run Solvers

To execute your solution for a given year and day, run:

```bash
cd src/AdventOfCode.Solutions
dotnet run -- run 2024 1
```

This will execute the `Part01` and `Part02` methods in the specified solution class and display the results.

---

### 3️⃣ Upload an Answer

When you're ready to submit an answer for a specific day, use:

```bash
cd src/AdventOfCode.Solutions
dotnet run -- upload 2024 1
```

The CLI will:
- Locate the most recent unsolved part for the specified day.
- Submit your answer to the Advent of Code website.

---

## 🤝 Contributing

Feel free to fork the repository, submit pull requests, or suggest features. Feedback is always welcome!

## 📝 License

This project is licensed under the [MIT License](LICENSE).

---

What do you think?
