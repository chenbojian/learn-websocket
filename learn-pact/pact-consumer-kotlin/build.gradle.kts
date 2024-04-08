val ktor_version: String by project

plugins {
    application
    kotlin("jvm") version "1.9.23"
    //https://github.com/ktorio/ktor-documentation/blob/2.3.9/codeSnippets/snippets/client-json-kotlinx/build.gradle.kts
    //https://github.com/Kotlin/kotlinx.serialization
    kotlin("plugin.serialization") version "1.9.23"
}

group = "org.example"
version = "1.0-SNAPSHOT"

repositories {
    mavenCentral()
}

dependencies {
    implementation("io.ktor:ktor-client-core:$ktor_version")
    implementation("io.ktor:ktor-client-cio:$ktor_version")
    implementation("io.ktor:ktor-client-content-negotiation:$ktor_version")
    implementation("io.ktor:ktor-serialization-kotlinx-json:$ktor_version")
    testImplementation(kotlin("test"))
    testImplementation("au.com.dius.pact.consumer:junit5:4.6.8")
    testImplementation("org.jetbrains.kotlinx:kotlinx-coroutines-test:1.8.0")
}

tasks.test {
    useJUnitPlatform()
    systemProperties["pact.rootDir"] = layout.buildDirectory.dir("custom-pacts-directory").get().toString()
}
kotlin {
    jvmToolchain(17)
}