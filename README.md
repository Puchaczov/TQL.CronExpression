# CronExpression
Parser and evaluator for CRON expressions. In assumption, this library will expose complete implementation to parse and evaluate EVERY expression that is valid and makes sense.

Short description:
Library is constructed in a way which should allow you to easily write your own evaluators and extensions based on parser produced AST.

<b>Solution is divided into four projects:</b><br><br>
1. Cron.Parser - Contains lexer and parser which produce Abstract Syntax Tree. <br>
2. Cron.Visitors - Contains visitors which checks if produced AST is valid and if so, expand produced AST for evaluation purposes <br>
3. Cron.Parser.Tests - Contains tests for parser. <br>
4. Cron.Visitors.Tests - Contains tests for visitors. <br>

<b>Evaluation:</b> <br><br>
In general, you can divide evaluation in 3 simple phases:<br>
1. Pass to parser your cron expression (you will receive AST in result)<br>
2. Run visitor which will check if your cron expression is valid OR run visitor which will check validity of expression and create evaluator based on AST.<br>
3. Get evaluator from visitor and calculate Next/Prev FireTime or whatever you want evaluate based on CRON exp (e.g sentence).

Current state:

<table>
  <thead>
    <tr>
      <td>Feature</td>
      <td>Parser</td>
      <td>Expression Validator</td>
      <td>Evaluator</td>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>*</td>
      <td>Supported</td>
      <td>Supported</td>
      <td>Supported</td>
    </tr>
    <tr>
      <td>,</td>
      <td>Supported</td>
      <td>Supported</td>
      <td>Supported</td>
    </tr>
    <tr>
      <td>-</td>
      <td>Supported</td>
      <td>Supported</td>
      <td>Supported</td>
    </tr>
    <tr>
      <td>/</td>
      <td>Supported</td>
      <td>Supported</td>
      <td>Supported</td>
    </tr>
    <tr>
      <td>?</td>
      <td>Supported</td>
      <td>Supported(DayOfMonth,DayOfWeek)</td>
      <td>Supported(DayOfMonth,DayOfWeek)!</td>
    </tr>
    <tr>
      <td>L</td>
      <td>Supported</td>
      <td>Supported</td>
      <td>Supported!</td>
    </tr>
    <tr>
      <td>W</td>
      <td>Supported</td>
      <td>Supported(DayOfMonth)</td>
      <td>Supported(DayOfMonth)</td>
    </tr>
    <tr>
      <td>LW</td>
      <td>Supported</td>
      <td>Supported(DayOfMonth)</td>
      <td>Supported(DayOfMonth)</td>
    </tr>
    <tr>
      <td>#</td>
      <td>Supported</td>
      <td>Supported(DayOfWeek)</td>
      <td>Supported(DayOfWeek)</td>
    </tr>
    <tr>
      <td>Pretty Exception Handling</td>
      <td>Partially Supported</td>
      <td>Partially Supported</td>
      <td>Not Applicable</td>
    </tr>
  </tbody>
</table>

! - require more tests<br>

<b>Pretty (compiler like) exception handling:</b> <br><br>
Feature, that allow you to recognize which part of expression is incorrect, additionaly it will produce meaningfull errors to make easier write and reform complex expressions. <br><br>

Currently, there is over one hundred unit tests for parser, visitors and evaluators. It is not so big amount and there will be much more I quess becouse there is still a lot of untested cases.<br>
Also in the near future, I see the need to create long-test-environment and allow evaluator be tested in such cases.
