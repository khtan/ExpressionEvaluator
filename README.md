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

### Additional assumptions
1) Numbers are base10
2) The input is like a 4-function calculator with 8 digits of display. 
   Keys to enter input are 0 to 9 numbers, decimal point, braces and RET.
   There is no entry for scientific notation, although the results may show up in that notation.
   There is no entry for very large symbolic numbers such as int.MaxValue
3) The operators "+" and "*" are binary operators. The addition of other operations would also be binary, ie
   "-" and "/". These can be conditionally compiled with the pragma #define MOREOPERATORS.
4) Unary operators are excluded for now.
   Leading signs such as +5 and -5 are considered unary operators.
   Other unary operators are sqrt etc.
   The code can accomodate unary operators and their precedence but it is less trivial.

### Implementation details
* Version Control : [Github](https://github.com/khtan/ExpressionEvaluator)
* Continuous Integration : [CircleCI](https://circleci.com/gh/khtan/ExpressionEvaluator**
* Test framework : console and XUnit.Net
* Software environment : dotnet 3.1 and Visual Studio 2019 Community

### Basic build information
Getting the code:
   > git clone https://github.com/khtan/ExpressionEvaluator.git

Building the code with VS Community 2019:
   Open the ExpressionEvaluator.sln file and do a build.
   To run tests, click on the Test/Test Explorer and then select Run All Tests or Run as desired.

Building the code with dotnet:
There is also a way to build/run on the dotnet command line. This is how CircleCi does it.
   Change directory in the shell to the root of the project
   > dotnet restore // to pull in the dependencies
   > dotnet test // to run all the tests

console\KweeConsole\bin\Debug\netcoreapp3.1\KweeConsole.exe
functionaltest\FunctionalSuite1\bin\Debug\netcoreapp3.1/FunctionalSuite1.dll
