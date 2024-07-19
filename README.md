# TestApp (Angular+.NET)
Simple web page that should contain a registration wizard. The registration wizard includes two steps.
* Angular
* .NET8
* ORM EF with migrations
* Microservices

## How to start
Open in VisualStudio and press F5. The solution is configured to run several projects at the same time (RegistrationService, LocationService and InnerTokenService).

## Dependencies
* Consul
* nginx
  config nginx:
  
        server {
                listen       8080;
                server_name  localhost;
    
            		location /consul/ {
            			proxy_pass http://localhost:8500/;
            			proxy_set_header Host $host;
            			proxy_set_header X-Real-IP $remote_addr;
            			proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
            			proxy_set_header X-Forwarded-Proto $scheme;
            		}
        }
  



