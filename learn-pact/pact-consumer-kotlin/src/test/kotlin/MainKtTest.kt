import au.com.dius.pact.consumer.MockServer
import au.com.dius.pact.consumer.dsl.PactDslWithProvider
import au.com.dius.pact.consumer.junit5.PactConsumerTestExt
import au.com.dius.pact.consumer.junit5.PactTestFor
import au.com.dius.pact.core.model.PactSpecVersion
import au.com.dius.pact.core.model.RequestResponsePact
import au.com.dius.pact.core.model.V4Pact
import au.com.dius.pact.core.model.annotations.Pact
import kotlinx.coroutines.delay
import kotlinx.coroutines.runBlocking
import kotlinx.coroutines.test.runTest
import kotlinx.serialization.encodeToString
import kotlinx.serialization.json.*
import org.apache.hc.client5.http.fluent.Request
import org.apache.hc.core5.http.HttpResponse
import org.example.getCustomer
import org.example.models.Customer
import org.junit.jupiter.api.extension.ExtendWith
import java.io.IOException
import kotlin.test.Test
import kotlin.test.assertEquals


@ExtendWith(PactConsumerTestExt::class)
@PactTestFor(providerName = "Provider", port = "8080")
class MainKtTest {

    @Pact(provider = "Provider", consumer = "Consumer")
    fun createPact(builder: PactDslWithProvider): RequestResponsePact {
        return builder
            .given("test state")
            .uponReceiving("ExampleJavaConsumerPactTest test interaction")
            .path("/customer/888")
            .method("GET")
            .willRespondWith()
            .status(200)
//            .headers(mapOf("Content-Type" to "application/json"))
            .bodyMatchingContentType("application/json", Json.encodeToString(mapOf(
                "id" to "1",
                "firstName" to "first",
                "lastName" to "last",
                "email" to "email"
            )))
            .toPact()
    }

    @Test
    @PactTestFor(pactMethod = "createPact", pactVersion = PactSpecVersion.V3)
    @Throws(IOException::class)
    fun test1(mockServer: MockServer) {
        val httpResponse: HttpResponse = Request.get("http://localhost:8080/customer/888").execute().returnResponse()
        assertEquals(httpResponse.code, 200)
    }

    @Test
    @PactTestFor(pactMethod = "createPact", pactVersion = PactSpecVersion.V3)
    fun test2(mockServer: MockServer) {
        runTest {
            getCustomer()
        }
    }

//    @Test
//    @PactTestFor(pactMethod = "createPact", pactVersion = PactSpecVersion.V3)
//    fun test3(mockServer: MockServer) {
//        runBlocking {
//            delay(30000)
//        }
//    }


}