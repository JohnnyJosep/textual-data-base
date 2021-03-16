FROM openjdk:17-jdk-alpine

COPY freeling-0.0.1-SNAPSHOT.jar /freelingapi.jar

EXPOSE 8080

ENTRYPOINT ["java","-jar","/freelingapi.jar"]