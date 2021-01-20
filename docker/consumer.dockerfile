FROM openjdk:8-jdk-alpine


# Run the jar file
#ENTRYPOINT ["java","-Djava.security.egd=file:/dev/./urandom","-jar","/spring-mysql-rabbitmq-docker-example.jar"]