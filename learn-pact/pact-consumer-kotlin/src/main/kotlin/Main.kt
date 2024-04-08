package org.example
import io.ktor.client.*
import io.ktor.client.call.*
import io.ktor.client.engine.cio.*
import io.ktor.client.plugins.contentnegotiation.*
import io.ktor.client.request.*
import io.ktor.client.statement.*
import io.ktor.serialization.kotlinx.json.*
import org.example.models.Customer


suspend fun main() {
    getCustomer()
    println("Hello World!")
}

suspend fun getCustomer() {
    val client = HttpClient(CIO) {
        install(ContentNegotiation) {
            json()
        }
    }
    val response: HttpResponse = client.get("http://localhost:8080/customer/888")
    val customer: Customer = response.body()
    println(customer)
    client.close()
}