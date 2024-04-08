plugins {
    id("org.gradle.toolchains.foojay-resolver-convention") version "0.5.0"
}
rootProject.name = "learn-pact"

include("pact-consumer-kotlin")
include("pact-provider-spring-kotlin")
