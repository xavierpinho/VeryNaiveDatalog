# VeryNaiveDatalog

This repository contains a naive, bottom-up implementation of Datalog's
semantics in about 200 lines of C# (.NET 5) code.

## Introduction

A Datalog program consists of a set of *facts* and *rules*. Facts denote 
assertions, while rules denote relationships (from which new facts 
are obtainable.)

The "Hello World" of Datalog is graph reachability:
```datalog
// Rules
ancestor(x,y) :- parent(x,y).
ancestor(x,y) :- ancestor(x,z), parent(z,y).

// Facts
parent(Homer, Lisa).
parent(Homer, Bart).
parent(Grampa, Homer).
```

For the snippet above, running the query `?ancestor(x, Bart)` (in English:
for which values of `x` does `ancestor(x,Bart)` hold?) would output 2
results, namely:

* `x = Homer`
* `x = Grampa`

which are obtainable only by first deriving the facts 
`ancestor(Grampa,Homer)`, `ancestor(Homer, Bart)` and 
`ancestor(Grampa,Bart)`, by repeated application of the rules to the facts.


## How to use

The snippet above can be encoded in VeryNaiveDatalog as follows (see `EvaluatorTests.cs`):

```c#
// Rules
var r0 = new Rule(new Atom("ancestor", new Variable("x"), new Variable("y")),
                new Atom("parent", new Variable("x"), new Variable("y")));

var r1 = new Rule(new Atom("ancestor", new Variable("x"), new Variable("y")),
                new Atom("ancestor", new Variable("x"), new Variable("z")),
                new Atom("parent", new Variable("z"), new Variable("y")));

var rules = new[]{r0, r1};

// Facts
var f0 = new Atom("parent", new Symbol("Homer"), new Symbol("Lisa"));
var f1 = new Atom("parent", new Symbol("Homer"), new Symbol("Bart"));
var f2 = new Atom("parent", new Symbol("Grampa"), new Symbol("Homer"));

var facts = new[]{f0, f1, f2};

// Query
var q = new Atom("ancestor", new Variable("x"), new Symbol("Bart"));

// Run
var result = facts.Query(q, rules);
```

## License

Public domain (see `UNLICENSE`.)

## References

* [The Essence of Datalog](https://dodisturb.me/posts/2018-12-25-The-Essence-of-Datalog.html) in Mistral Contrastin's blog.