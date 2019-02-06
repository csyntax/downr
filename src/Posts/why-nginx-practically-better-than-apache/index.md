---
title: Why NGINX Practically Better Than Apache
slug: why-nginx-practically-better-than-apache
date: 20-07-2018
categories: Nginx
---

![NGINX vs Apache](media/nginx-vs-apache.png)

**Apache** is a free, open-source _HTTP_ server for _Unix-like_ operating systems and _Windows_.
It was designed to be a secure, efficient and extensible server that provides HTTP services in sync with the prevailing HTTP standards.

Ever since it’s launch, **Apache** has been the most popular web server on the Internet since 1996.
It is the de facto standard for Web servers in the _Linux_ and _open source ecosystem_.
New Linux users normally find it easier to set up and use.

**Nginx** (also know as **"Engine-X"**) is a free, open-source, high-performance HTTP server, reverse proxy, and an IMAP/POP3 proxy server.
Just like **Apache**, it also runs on _Unix-like_ operating systems and _Windows_.

Well known for it’s high performance, stability, simple configuration, and low resource consumption, it has over the years become so popular and its usage on the Internet is heading for greater heights.
It is now the web server of choice among experienced system administrators or web masters of top sites.

**Some popular web applications powered by:**

* **Apache** are: PayPal, BBC.com, BBC.co.uk, SSLLABS.com, Apple.com plus lots more.
* **Nginx** are: Netflix, Udemy.com, Hulu, Pinterest, CloudFlare, WordPress.com, GitHub, SoundCloud and many others.

I will simply share my experience and thoughts about the whole debate, having tried out _Apache_ and _Nginx_, both in production environments based on requirements for hosting modern web applications,
in the next section.

## Nginx is Lightweight

**Nginx** is one of light weight web servers out there.
It has small footprints on a system compared to Apache which implements a vast scope of functionality necessary to run an application.

Because Nginx puts together a handful of core features,
it relies on dedicated third‑party upstream web servers such
as an Apache backend, _FastCGI_, _Memcached_, _SCGI_, and _uWSGI_ servers or application server,
i.e language specific servers like *Node.js*, *Tomcat*, etc.

Therefore its memory usage is far better suited for limited resource deployments, than **Apache**.

## Nginx is Designed for High Concurrency

As opposed to _Apache’s_ threaded or process-oriented architecture (process‑per‑connection or thread‑per‑connection model), *Nginx* uses a scalable, event-driven (asynchronous) architecture.
It employs a liable process model that is tailored to the available hardware resources.

It has a master process (which performs the privileged operations such as reading configuration and binding to ports) and which creates several worker and helper processes.

The worker processes can each handle thousands of HTTP connections simultaneously, read and write content to disk, and communicate with upstream servers. The helper processes (cache manager and cache loader) can manage on‑disk content caching operations.

This makes its operations scalable, and resulting into high performance.
This design approach further makes it fast, favorable for modern applications.
In addition, third‑party modules can be used to extend the native functionalities in **NGINX**.

## Nginx is Easy to Configure

**NGINX** has a simple configuration file structure, making it super easy to configure.
It consists of modules which are controlled by directives specified in the configuration file.
In addition, directives are divided into block directives and simple directives.

A block directive is defined by braces `{` and `}`.
If a block directive can have other directives inside braces, it is called a context such as events, http, server, and location.

```nginx
http {
    server {
        ...
    }
}
```

A simple directive consists of the name and parameters separated by spaces and ends with a semicolon `;`.

```nginx
http {
    server {
        location / {
            # This is simple directive called as root
            root  /var/www/hmtl/csyntax.com/;
        }
    }
}
```

You can include custom configuration files using the include directive, for example.

```nginx
http {
    server {
        ...
    }

    ## Examples of including additional config files
    include  /path/to/config/file/*.conf;
    include  /path/to/config/file/ssl.conf;
}
```

## Nginx is an Excellent Frontend Proxy

One of the common uses of *Nginx* is setting it up as a proxy server, in this case it receives _HTTP_ requests from clients and passes them to proxied or upstream servers that were mentioned above, over different protocols.

You can also modify client request headers that are sent to the proxied server, and configure buffering of responses coming from the proxied servers.

Then it receives responses from the proxied servers and passes them to clients.
It is mush easier to configure as a proxy server compared to *Apache* since the required modules are in most cases enabled by default.

## Nginx is Remarkable for Serving Static Content

Static content or files are typically files stored on disk on the server computer, for example **CSS styles**,**JavaScripts scripts** amd **images**.
Let’s consider a scenario where you using Nginx as a Front-End for Node.js (the application server).

Although **Node.js** server have built in features for static file handling, they don’t need to do some intensive processing to deliver non-dynamic content, therefore it is practically beneficial to configure the web server to serve static content directly to clients.

**NGINX** can perform a much better job of handling static files from a specific directory, and can prevent requests for static assets from choking upstream server processes.
This significantly improves the overall performance of backend servers.

## Nginx is Highly Scalable

Furthermore, **NGINX** is highly scalable and modern web applications especially enterprise applications demand for technology that provides high performance and scalability.

One company benefiting from Nginx’s amazing scalability features is __CloudFlare__,
it has managed to scale its web applications to handle more than 15 billion monthly page views with a relatively modest infrastructure, according to __Matthew Prince__, Co-Founder and CEO of __CloudFare__.

## Nginx is an Efficient Load Balancer

To realize high performance and uptime for modern web applications may call for running multiple application instances on a _single_ or _distributed HTTP servers_.
This may in turn necessitate for setting up load balancing to distribute load between your _HTTP servers_.

Today, load balancing has become a widely used approach for optimizing operating system resource utilization, maximizing flexibility, cutting down latency, increasing throughput, achieving redundancy, and establishing fault-tolerant configurations – across multiple application instances.

* **Round-robin (default method)** – requests to the upstream servers are distributed in a round-robin fashion (in order of the list of servers in the upstream pool).
* **Least-connected** – here the next request is proxied to the server with the least number of active connections.
* **IP-hash** – here a hash-function is used to determine what server should be selected for the next request (based on the client’s IP address).
* **Least time (only Nginx Plus)** – assigns the next request to the upstream server with the least number of current connections but favors the servers with the lowest average response times.
* **Generic hash** – under this method, the system administrator specifies a hash (or key) with the given text, variables of the request or runtime, or their combination. __For example, the key may be a source IP and port, or URI.__
_NGINX_ then distributes the load amongst the upstream servers by generating a hash for the current request and placing it against the upstream servers.

## Conclusion

Both **Apache** and **NGINX** can’t be replaced by each other, they have their strong and weak points.
However, **NGINX** offers a powerful, flexible, scalable and secure technology 
for reliably and efficiently powering modern websites and web applications.
__What is your take? Let us know via the feedback form below.__

![NGINX vs Apache requests per second](media/nginx-apache-reqs-sec.png)
