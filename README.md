[![Stories in Ready](https://badge.waffle.io/Puchaczov/CronExpression.png?label=ready&title=Ready)](https://waffle.io/Puchaczov/CronExpression)
# CronExpression

<h2 class="western" lang="en-US">What is CronExpression</h2>
<p lang="en-US">CronExpression is a .NET library provids CRON
expression analysis tools and evaluation environment. By this library
you can for example process your expression in a way that allows you
to judge provided expression correctness. You can use it as
evaluation engine when you build scheduler. Software should be useful
for everybody who needs to provide own translation from one
expression to other expression. For example, you could write an
extension that convert expression to human readable sentence.</p>
<h2 class="western" lang="en-US">Goals</h2>
<p lang="en-US">Provide tool that allows you to work with any kind of
expression, no matter how complex would it be.</p>
<h2 class="western" lang="en-US">Latest release</h2>
<p lang="en-US">There is no stable release yet. Currently the most
stable version is 0.9 which is “just before release”
version. There won’t be new features before 1.0 release. It’s
time to focus to make it the most reliable although already <b>there
is no known issues.</b> Things left to make stable release are:</p>
<ul>
	<li>
<p><span lang="en-US">Refactorizations.</span></p>
	</li><li>
<p><span lang="en-US">Check performance of generating new
	dates in evaluator.</span></p>
	</li><li>
<p><span lang="en-US">Create Nuget packages</span></p>
	</li><li>
<p><span lang="en-US">More tests for evaluator</span></p>
</li></ul>
<h2 class="western" lang="en-US">What is supported</h2>
<p lang="en-US">Currently, this library has support for standard
defined expression and some of nonstandard definitions.</p>
<p lang="en-US">Supported nonstandard definitions are:</p>
<ul>
	<li>
<p><span lang="en-US"><b>@annual</b></span></p>
	</li><li>
<p><span lang="en-US"><b>@yearly</b></span></p>
	</li><li>
<p><span lang="en-US"><b>@monthly</b></span></p>
	</li><li>
<p><span lang="en-US"><b>@weekly</b></span></p>
	</li><li>
<p><span lang="en-US"><b>@daily</b></span></p>
	</li><li>
<p><span lang="en-US"><b>@hourly</b></span></p>
</li></ul>
<p lang="en-US">Standard definitions are:</p>
<table cellpadding="0" cellspacing="0">
	<thead>
		<tr>
			<td style="border: 1.00pt solid #dddddd; padding: 0.05cm" width="103">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Feature</font></font></p>
			</td>
			<td style="border-top: 1.00pt solid #dddddd; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0.05cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="104">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Parser</font></font></p>
			</td>
			<td style="border-top: 1.00pt solid #dddddd; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0.05cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="172">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Expression
				Validator</font></font></p>
			</td>
			<td style="border-top: 1.00pt solid #dddddd; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0.05cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="224">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Evaluator</font></font></p>
			</td>
		</tr>
	</thead>
	<tbody>
		<tr>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: 1.00pt solid #dddddd; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0.05cm; padding-right: 0.05cm" width="103">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">*</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="104">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="172">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="224">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
		</tr>
		<tr>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: 1.00pt solid #dddddd; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0.05cm; padding-right: 0.05cm" width="103">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">,</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="104">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="172">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="224">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
		</tr>
		<tr>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: 1.00pt solid #dddddd; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0.05cm; padding-right: 0.05cm" width="103">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">-</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="104">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="172">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="224">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
		</tr>
		<tr>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: 1.00pt solid #dddddd; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0.05cm; padding-right: 0.05cm" width="103">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">/</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="104">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="172">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="224">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
		</tr>
		<tr>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: 1.00pt solid #dddddd; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0.05cm; padding-right: 0.05cm" width="103">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">?</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="104">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="172">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported(DayOfMonth,DayOfWeek)</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="224">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported(DayOfMonth,DayOfWeek)</font></font></p>
			</td>
		</tr>
		<tr>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: 1.00pt solid #dddddd; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0.05cm; padding-right: 0.05cm" width="103">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">L</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="104">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="172">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="224">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
		</tr>
		<tr>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: 1.00pt solid #dddddd; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0.05cm; padding-right: 0.05cm" width="103">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">W</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="104">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="172">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported(DayOfMonth)</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="224">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported(DayOfMonth)</font></font></p>
			</td>
		</tr>
		<tr>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: 1.00pt solid #dddddd; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0.05cm; padding-right: 0.05cm" width="103">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">LW</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="104">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="172">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported(DayOfMonth)</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="224">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported(DayOfMonth)</font></font></p>
			</td>
		</tr>
		<tr>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: 1.00pt solid #dddddd; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0.05cm; padding-right: 0.05cm" width="103">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">#</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="104">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="172">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported(DayOfWeek)</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="224">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported(DayOfWeek)</font></font></p>
			</td>
		</tr>
		<tr>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: 1.00pt solid #dddddd; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0.05cm; padding-right: 0.05cm" width="103">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Errors</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="104">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="172">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="224">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Not
				Applicable</font></font></p>
			</td>
		</tr>
	</tbody>
</table>
<p>&nbsp;</p>
<h2 class="western" lang="en-US">How it’s build</h2>
<p lang="en-US">The way how this library had been written make that
you can distinguish four main phases (they are properly ordered)</p>
<ol>
	<li>
<p><span lang="en-US">Preprocessing – The very first
	phase that allows your expression to be unwinded to form that can be
	handle by parser.</span></p>
	</li><li>
<p><span lang="en-US">Lexical analysis – Perform
	analysis by which you will receive stream of tokens – these
	are properly recognized basic elements of expression. Your
	expression consists of this basics. For example, when you type
	MON-WED, you will receive three tokens – [Name, Range, Name]</span></p>
	</li><li>
<p><span lang="en-US">Parsing – Process by which stream
	of tokens is converted to tree.</span></p>
	</li><li>
<p><span lang="en-US">Semantic analysis – Process where
	</span><span lang="en-US">the decision is made</span><span lang="en-US">
	if each part of expression applied to defined rules. This is place
	where your expression can be expanded in evaluation purposes.</span></p>
</li></ol>
<p style="margin-left: 0.08cm" lang="en-US">This knowledge will be
interesting for you in case when you will build your own evaluator or
analysis tool based on this library (<b>for more, check
documentation)</b>.</p>
<h2 class="western" lang="en-US">Errors</h2>
<p lang="en-US">This library behave a bit like compiler. When
malformed expression were being written, you will be informed about
that by produced errors. It took much efforts to produce meaningful
errors in case of error. If you think, that some messages aren’t
meaningful enough or there are rules that isn’t applied when
rules are checked – use <b>issues </b>to inform us or
<b>contribute </b>to project. In general, I have hope to inform and
help you reform your expression (<b>for more, check documentation)</b></p>
<h2 class="western" lang="en-US">Tests</h2>
<p lang="en-US">In general, each part described in section <b>how
it’s build </b>contains own tests. I try makes efforts to make
this software reliable. If you notice bugs, please report it in
<b>issues </b>section. I appreciate if you will contribute too.</p>
<h2 class="western" lang="en-US">Branches</h2>
<p lang="en-US">There is two branches you can write your fixes. I
will describe it for a moment.</p>
<ul>
	<li>
<p><span lang="en-US">Main – branch for bug fixes in
	currently released version.</span></p>
	</li><li>
<p><span lang="en-US">Development – Branch for fixes and
	new features. </span>
	</p>
</li></ul>
<h2 class="western" lang="en-US">API &amp; Documentation</h2>
<p lang="en-US">I took much effort to make this software easy to use.
Generally, to successfully use this library, you won’t need to
learn a lot, just read my short introduction in appropriate section
of documentation (<b>go here</b>). If you would like to bring
something new to library, read <b>documents</b> here.</p>
<h2 class="western" lang="en-US">Contribute</h2>
<p lang="en-US">If you like this library, please contribute and
appreciate our efforts. You can do it by creating fixes, creating new
features, reporting issues or starring us. Feel free to report ideas,
at least there is good place to consider implementation of it.</p>
<h2 class="western" lang="en-US">License</h2>
<p lang="en-US">This library is based on <b>MIT </b>license, please
read <b>LICENSE </b>when you would like to know more.</p>
