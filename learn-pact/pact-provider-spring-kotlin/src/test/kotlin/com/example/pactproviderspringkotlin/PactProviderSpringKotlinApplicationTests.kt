package com.example.pactproviderspringkotlin

import au.com.dius.pact.provider.junit5.HttpTestTarget
import au.com.dius.pact.provider.junit5.PactVerificationContext
import au.com.dius.pact.provider.junit5.PactVerificationInvocationContextProvider
import au.com.dius.pact.provider.junitsupport.Provider
import au.com.dius.pact.provider.junitsupport.State
import au.com.dius.pact.provider.junitsupport.loader.PactFolder
import org.junit.jupiter.api.BeforeEach
import org.junit.jupiter.api.TestTemplate
import org.junit.jupiter.api.extension.ExtendWith
import org.springframework.boot.test.context.SpringBootTest
import org.springframework.boot.test.web.server.LocalServerPort
import org.springframework.test.context.junit.jupiter.SpringExtension

@Provider("Provider")
@PactFolder("pacts")
//@ExtendWith(SpringExtension::class)
@SpringBootTest(webEnvironment = SpringBootTest.WebEnvironment.RANDOM_PORT)
//@SpringBootTest
class PactProviderSpringKotlinApplicationTests {

	@LocalServerPort
	var port: Int = 0

	@BeforeEach
	fun before(context: PactVerificationContext) {
		context.target = HttpTestTarget("localhost", port)
	}

	@TestTemplate
	@ExtendWith(PactVerificationInvocationContextProvider::class)
	fun verifyPact(context: PactVerificationContext) {
		context.verifyInteraction()
	}

	@State("test state")
	fun test() {
	}

}
