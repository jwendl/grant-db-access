# Grant Database Access

*Problem Statement*:

Working with deployment scripts in bicep gets kind of tricky. Especially, when attempting to build an end-to-end solution with SQL Azure.

This repository is an attempt to fix that. 

*Solution*:

Essentially, SQLCMD and GO-SQLCMD have odd implementations of Managed Identity that does not work with deployment scripts. Because of this, we decided to write a simple command line that would run on deployment scripts to do things like grant a managed identity access to a SQL Azure database.
