package com.example.pactproviderspringkotlin

import com.example.pactproviderspringkotlin.models.Customer
import org.springframework.http.ResponseEntity
import org.springframework.web.bind.annotation.GetMapping
import org.springframework.web.bind.annotation.PathVariable
import org.springframework.web.bind.annotation.RestController
import java.util.*

@RestController
class CustomerController {
    @GetMapping("customers")
    fun getAllCustomers(): List<Customer> {
        return listOf()
    }

    @GetMapping("customer/{id}")
//    fun getCustomerById(@PathVariable("id") id: String?): ResponseEntity<Customer> {
//        val customer: Optional<Customer> = Optional.of(Customer(id = "888", firstName = "first", lastName = "last", email = "email"))
//        return ResponseEntity.of(customer)
//    }
    fun getCustomerById(@PathVariable("id") id: String?): Customer {
        return Customer(id = "888", firstName = "first", lastName = "last", email = "email")
    }
}