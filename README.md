[![CircleCI Status](https://circleci.com/gh/khtan/ExpressionEvaluator.svg?style=shield)](https://circleci.com/gh/khtan/ExpressionEvaluator)
# ExpressionEvaluator
Simple expression evaluator and tests
## Introduction
This is the programming assignment for an SDET Test and Automation position.
### Problem statement
 Write a program to evaluate arithmetic expressions.  Input will be text expression strings.  Here are some examples:
 5 + 14
 (8 + 2) * 4
 7 + 3 + 9
 (6 + 5) * (8 + 2)

 Your solution should:
 * Demonstrate the ability to parse/evaluate arithmetic expressions
 * Support addition and multiplication and be easily extensible to add other operations later
 * Be designed and implemented in an object-oriented manner
 * Not use the "Shunting Yard" algorithm
 * Contain a testing framework to validate that the solution is functioning as desired
 * Use whatever tools and languages you are most comfortable with
### Clarifications
The main clarification was on why/whether the Shunting Yard (SY) algorithm should not be used.

> The requirement asked that I not use SY. The wiki page indicates SY is an algorithm for parsing mathematicalÂ  expressions specified in infix notation. It can also
> be used to convert an infix notation to a pre/post fix notation.
> Sedgewick calls it Dijkstra's Two-Stack algorithm. Many online examples use SY without mentioning it. The tell-tale sign is probably the single pass that
> makes it ideal in implementation and memory resource.
>
> Anyway, I gather the requirement not to use SY boils down to :
> 1) don't convert to infix/postfix
> 2) don't use 2 stacks.
>
> Is that your intent?

The intent is that you not copy a readily available solution from the Internet. We want to see an approach that you design and implement.
It is okay for you to use available language libraries to help you out.
For example, if the language you use has libraryÂ classes/functions for breaking a string into tokens, you can use the library and not write a tokenizer from scratch.

## Additional assumptions
1) Numbers are base10
2) The input is like a 4-function calculator with 8 digits of display. 
   Keys to enter input are 0 to 9 numbers, decimal point, braces and RET.
   There is no entry for scientific notation, although the results may show up in that notation.
   There is no entry for very large symbolic numbers such as int.MaxValue
3) The operators "+" and "*" are binary operators. The addition of other operations would also be binary, ie
   "-" and "/**. These can be conditionally compiled with the pragma #define MOREOPERATORS.
4) Unary operators are excluded for now.
   Leading signs such as +5 and -5 are considered unary operators.
   Other unary operators are sqrt etc.
   The code can accomodate unary operators and their precedence but it is less trivial.

## Preliminary research
Initial research suggests that for industrial strength parsing, it is advisable to use something like Antlr.
Since this project is for learning, I found a lightweight C# parser that I thought I could leverage upon.
It is called [Sprache]( https://github.com/sprache/Sprache). A separate article ["Sprache.Calc: Building Yet Another Expression Evaluator"](https://www.codeproject.com/Articles/795056/Sprache-Calc-Building-Yet-Another-Expression-Evalu) even gave the implementation for a simple calculator.  Instead of just coping the pattern, I decided to adopt this as a test buddy. Eventually, it would then be used to compare its output
to that of the expression evaluator I will write.

I tried to devise an algorithm for evaluating an arbitary infix expression and ran into many problems.
After clarifying the reasons for being able to reimplement the SY algorithm, I decided that it would be
safe to re-implement it from Sedgewick's 4 line description ( Sedgewicks Algorithms 4th edition, Chapter 1
Section 1.3 on 'Arithmetic expression evaluation') 
   1. Push operands onto the operand stack.
   2. Push operators onto the operator stack.
   3. Ignore left parentheses.
   4. On encountering a right parenthesis, pop an operator, pop the requisite number of operands, and push onto the operand stack the result of applying that operator to those operands.

There is also a complete java code implementation for reference.

## Implementation details
* Version Control : [Github](https://github.com/khtan/ExpressionEvaluator)
* Continuous Integration : [CircleCI](https://circleci.com/gh/khtan/ExpressionEvaluator**
* Test framework : console and XUnit.Net
* Software environment : dotnet 3.1 and Visual Studio 2019 Community

## Basic build information
**Getting the code:**
   > git clone https://github.com/khtan/ExpressionEvaluator.git

**Building the code with VS Community 2019:**

   Open the ExpressionEvaluator.sln file and do a build.
   To run tests, click on the Test/Test Explorer and then select Run All Tests or Run as desired.

**Building the code with dotnet:**

   There is also a way to build/run on the shell, using the dotnet command line. This is how CircleCi does it.
   Change directory in the shell to the root of the project
   > dotnet restore // to pull in the dependencies

   > dotnet test // to run all the tests
   
   If you need to install dotnet core, look at [Microsoft Install .NET Core on Windows](https://docs.microsoft.com/en-us/dotnet/core/install/windows?tabs=netcore31). There is a button to "Download .NET Core" which can be run to install.
   
## Design
### The functional API

Given that we have one function to implement, it would seeem that the api would be quite simple. In C# parlance,
it would be Func<string, Double). While that is minimal, it would be more useful to consider error conditions as well.

Below is an excerpt from Calc.cs that implements this functional interface.

    /// <summary>
    /// The fundamental functional interface is Func<string, Tuple<dynamic?, string?>>, ie
    /// the input is an expression ( string )
    /// and the output is a Tuple. 
    /// The first value of the Tuple is a nullable dynamic, representing the value of the expression, if it evaluates without error.
    /// The null would represents a bad evaluation.
    /// The second value of the Tuple is a nullable string, representing the error message if any.
    /// The null represents no errors.
    /// 
    /// This library provides two implementations of the expression evaluator.
    /// The KweeCalc is written by yours truly.
    /// The SimpleCalculator is a test buddy.
    /// </summary>

### Code/Test organization

The code is organized into 3 folders :
#### 1. lib

This contains the source code for the libraries.

Each library <libraryName> has its accompanying <libraryName>.test that is its companion unit tests.

    Library                | Description
    -----------------------|---------------------------------------------------------------------------------
    KweeLib                | Implementation for Expression Evaluator
    KweeLib.test           | unit tests for KweeLib
    SpracheLib             | Test buddy from Sprache.Calc
    SpracheLib.test        | unit tests for SpracheLib
    FunctionalCalcLib      | Implementation of functional interface, to isolate from SpracheLib and KweeLib
    FunctionalCalcLib.test | unit tests for FunctionalCalcLib
    
#### 2. console
This provides console drivers that wraps the functionality in the libraries for convenient and direct use.
It also uses the functional interface instead of directly using the libraries.
It is a minimal console and does not have help options etc. 
It is useful for quick and manual testing, although it can be used in automation by simple file redirection.

   Console        | Path to executable
   ---------------|----------------------------------
   KweeConsole    | console\KweeConsole\bin\Debug\netcoreapp3.1\KweeConsole.exe
   SpracheConsole | console\SpracheConsole\bin\Debug\netcoreapp3.1\SpracheConsole.exe

#### 3. functionaltest

This is the functional test for ???

