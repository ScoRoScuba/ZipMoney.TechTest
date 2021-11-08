# ZipPay Tech Exercise

## Basic Instructions

Hopefully this should be painless
1. Clone the repo and or bring local
2. CD into the Folder that has the docker-compose.yml file in it
3. run docker-compose build then ***docker-compose up***.  If you dont want to see all the lovely messaging that comes out run ***docker-compose up -d***
4. You should now be able to navigate to http://localhost:5000 and it should come up on a swagger page, if not http://localhost:5000/swagger/ui
5. Once done to remove everything ***docker-compose rm -v*** (get rid of the volumes as well)

## Notes
This is using MySQL DB, that was the most fun, was playing around in this area anyway so .......
Is it production ready, IMHO, no.   It runs it does what it can to cover silly stuff but nothing major.  Biggest item missing is logging, so no tracing or tracking, that said you can run with docker-compose up and see the output.

